using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class SnakeGame : Game {
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public SnakeGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize() {
      ScreenManager.Initialize();
      graphics.PreferredBackBufferWidth = (int)ScreenManager.Dimensions.X;
      graphics.PreferredBackBufferHeight = (int)ScreenManager.Dimensions.Y;
      graphics.ApplyChanges();
      base.Initialize();
    }

    protected override void LoadContent() {
      ScreenManager.LoadContent(Content);
      SoundManager.LoadContent(Content);
      spriteBatch = new SpriteBatch(GraphicsDevice);
      base.LoadContent();
    }

    protected override void UnloadContent() {
      Content.Unload();
      base.UnloadContent();
    }

    protected override void Update(GameTime gameTime) {
      if (!IsActive) return;
      if (shouldQuit) Exit();
      ScreenManager.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Lavender);

      spriteBatch.Begin();
      ScreenManager.Draw(spriteBatch);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    #region static variables
    private static bool shouldQuit = false;

    public static void Quit() {
      shouldQuit = true;
    }
    #endregion
  }
}