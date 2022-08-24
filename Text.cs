using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace snake {
  public class Text {
    private readonly SpriteFont font;
    private readonly string content;
    private readonly Vector2 position;
    private readonly Vector2 size;
    private event Action OnClick;
    private event Action<Text> OnUpdate;

    public readonly bool isClickable;
    public Color Color;

    public Text(SpriteFont font, string content, Vector2 position) {
      this.font = font;
      this.content = content;
      size = font.MeasureString(content);
      Color = Color.Black;
      isClickable = false;
      OnClick = null;
      this.position = new(position.X - size.X / 2, position.Y - size.Y / 2);
    }

    public Text(SpriteFont font, string content, Vector2 position, Action onClick) : this(font, content, position) {
      isClickable = true;
      OnClick += onClick;
    }

    public Text(SpriteFont font, string content, Vector2 position, Action onClick, Action<Text> onUpdate) : this(font, content, position, onClick) {
      OnUpdate += onUpdate;
    }

    public void Update() {
      Color = Color.Black;
      if (InputManager.HasHoveredOverArea(position, size)) {
        Color = Color.Firebrick;
      }
       OnUpdate?.Invoke(this);
      if (InputManager.HasClickedOnArea(position, size) && OnClick != null) {
        SoundManager.Play(SoundManager.Sound.SELECT);
        OnClick.Invoke();
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.DrawString(font, content, position, Color);
    }
  }
}
