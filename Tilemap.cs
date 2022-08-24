using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace snake {
  public class Tilemap {
    private readonly Texture2D wall;
    private readonly Texture2D space;
    private readonly Tile[,] tiles;

    public List<Tile> Walls { get; private set; }
    public List<Tile> Spaces { get; private set; }

    public Tilemap(ContentManager content) {
      Walls = new();
      Spaces = new();
      wall = content.Load<Texture2D>("sprites\\wall_full");
      space = content.Load<Texture2D>("sprites\\space");
      
      int width = size.X;
      int height = size.Y;
      tiles = new Tile[width, height];
      char[,] levelData = new char[width, height];
     
      var lines = File.ReadLines("Content\\levels\\level" + Level + ".txt");
      int i = 0;
      int j = 0;

      Debug.Assert(lines.Count() == height);
      foreach (string line in lines) {
        var chs = line.ToCharArray();
        Debug.Assert(chs.Length == width);
        foreach (char ch in chs) {
          levelData[i, j] = ch;
          ++i;
        }
        i = 0;
        ++j;
      }

      for (i = 0; i < width; i++) {
        for (j = 0; j < height; j++) {
          char ch = levelData[i, j];

          Point point = new(i, j);
          Texture2D texture = ch == '.' ? space : wall;
          Tile.Type type = ch == '.' ? Tile.Type.SPACE : Tile.Type.WALL;
          Color color = ch == '.' ? Color.Lavender : Color.White;
          Tile tile = new(texture, point, color, type);
          tiles[i, j] = tile;

          if (ch == '.') {
            Spaces.Add(tile);
          } else {
            Walls.Add(tile);
          }
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      foreach (Tile tile in tiles) {
        tile.Draw(spriteBatch);
      }
    }

    #region static variables
    static Tilemap() {
      tileSize = new Vector2(32f, 32f);

      Vector2 screenSize = ScreenManager.Dimensions;
      int width = (int)(screenSize.X / tileSize.Y);
      int height = (int)(screenSize.Y / tileSize.Y);
      size = new Point(width, height);

      Debug.Assert(((int)(screenSize.X % tileSize.X)) == 0);
      Debug.Assert(((int)(screenSize.Y % tileSize.Y)) == 0);
    }

    public static readonly Vector2 tileSize;
    public static readonly Point size;

    public static int Level { get; set; }

    public static Vector2 PointToPosition(Point point) {
      return new Vector2(point.X * tileSize.X, point.Y * tileSize.Y);
    }

    public static Point PositionToPoint(Vector2 position) {
      return new Point((int)((position.X) / tileSize.X), (int)((position.Y / tileSize.Y)));
    }

    // c# modulo does not work for negative ie -1 % 5 != 4
    public static int Modulo(int n, int mod) {
      int r = n % mod;
      return r < 0 ? r + mod : r;
    }

    public static List<Point> GetFreePoints(IEnumerable<GameObject> spaces, IEnumerable<GameObject> snakeParts) {
      return ObjectsToPoints(spaces.Except(snakeParts).ToList());
    }

    private static List<Point> ObjectsToPoints(IEnumerable<GameObject> objects) {
      List<Point> points = new();
      foreach (GameObject obj in objects) {
        points.Add(obj.Point);
      }
      return points;
    }
    #endregion
  }
}
