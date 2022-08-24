using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
  public class Tile : GameObject
  {
    public Type type;
    public enum Type { WALL, SPACE }

    public Tile(Texture2D texture, Point point, Color color, Type type) : base(texture, point, color)
    {
      this.type = type;
    }

    public Tile(Texture2D texture, Point point, Type type) : base(texture, point) {
      this.type = type;
    }
  }
}
