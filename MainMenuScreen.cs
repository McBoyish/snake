using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class MainMenuScreen : Screen {
    private SpriteFont fontSmall;
    private SpriteFont fontMedium;
    private SpriteFont fontLarge;
    private Text[] texts;

    public override void Initialize() {
      base.Initialize();
      texts = new Text[5];
    }

    public override void LoadContent(ContentManager c) {
      base.LoadContent(c);

      fontSmall = content.Load<SpriteFont>("font\\retro_small");
      fontMedium = content.Load<SpriteFont>("font\\retro_medium");
      fontLarge = content.Load<SpriteFont>("font\\retro_large");

      float width = ScreenManager.Dimensions.X;
      float height = ScreenManager.Dimensions.Y;

      Vector2 titlePosition = new(width / 2, 0.1f * height);
      Vector2 playPosition = new(width / 2, 0.4f * height);
      Vector2 levelPosition = new(width / 2, 0.5f * height);
      Vector2 quitPosition = new(width / 2, 0.6f * height);
      Vector2 createdByPosition = new(width / 2, 0.95f * height);

      texts[0] = new Text(fontLarge, "Snake", titlePosition);
      texts[2] = new Text(fontMedium, "Play", playPosition, () => { ScreenManager.AddScreen(new GameScreen()); });
      texts[3] = new Text(fontMedium, "Levels", levelPosition, () => { ScreenManager.AddScreen(new LevelScreen()); });
      texts[1] = new Text(fontMedium, "Quit", quitPosition, () => SnakeGame.Quit());
      texts[4] = new Text(fontSmall, "Created by Jake Li", createdByPosition);
    }

    public override void UnloadContent() {
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime) {
      foreach (Text text in texts) {
        if (text.isClickable) {
          text.Update();
        }
      }
    }

    public override void Draw(SpriteBatch spriteBatch) {
      foreach (Text text in texts) {
        text.Draw(spriteBatch);
      }
    }
  }
}
