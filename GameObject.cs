using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace snake {
  public class GameObject : IEquatable<GameObject> {
    public Color Color { get; set; }
    public Texture2D Texture { get; set; }
    public Point Point { get; set; }

    public GameObject(Texture2D texture) {
      Texture = texture;
      Point = Point.Zero;
      Color = Color.White;
    }

    public GameObject(Texture2D texture, Point point) : this(texture) {
      Point = point;
    }

    public GameObject(Texture2D texture, Color color) : this(texture) {
      Color = color;
    }

    public GameObject(Texture2D texture, Point point, Color color) : this(texture) {
      Point = point;
      Color = color;
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(Texture, Tilemap.PointToPosition(Point), Color);
    }

    public bool Contains(Point point) {
      return point.Equals(Point);
    }

    public override bool Equals(object obj) {
      if (obj is GameObject @object) {
        return Equals(@object);
      }
      return false;
    }

    public bool Equals(GameObject other) {
      return Point.Equals(other.Point);
    }

    public override int GetHashCode() {
      return Point.GetHashCode();
    }
  }
}
