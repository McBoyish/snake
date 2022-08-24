using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class Screen {
    protected ContentManager content;

    public virtual void Initialize() {
      InputManager.Initialize();
    }

    public virtual void LoadContent(ContentManager content) {
      this.content = new ContentManager(content.ServiceProvider, "Content");
    }

    public virtual void UnloadContent() {
      content.Unload();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch) { }
  }
}
