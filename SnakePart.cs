using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class SnakePart : GameObject {
    public Type type;
    public enum Type { HEAD, BODY, TAIL }

    public SnakePart(Texture2D texture, Point point, Type type) : base(texture, point) {
      this.type = type;
    }
  }
}
