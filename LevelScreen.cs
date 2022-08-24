using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class LevelScreen : Screen {
    private SpriteFont fontMedium;
    private Text[] texts;

    public override void Initialize() {
      base.Initialize();
      texts = new Text[5];
    }

    public override void LoadContent(ContentManager c) {
      base.LoadContent(c);

      fontMedium = content.Load<SpriteFont>("font\\retro_medium");

      float width = ScreenManager.Dimensions.X;
      float height = ScreenManager.Dimensions.Y;

      Vector2 level1Pos = new(width / 2, 0.3f * height);
      Vector2 level2Pos = new(width / 2, 0.4f * height);
      Vector2 level3Pos = new(width / 2, 0.5f * height);
      Vector2 level4Pos = new(width / 2, 0.6f * height);
      Vector2 backPos = new(width / 2, 0.7f * height);

      texts[0] = new Text(fontMedium, "Level 1", level1Pos,
        () => { Tilemap.Level = 1; },
        (Text text) => { if (Tilemap.Level == 1) text.Color = Color.DarkGreen; });

      texts[1] = new Text(fontMedium, "Level 2", level2Pos,
        () => { Tilemap.Level = 2; },
        (Text text) => { if (Tilemap.Level == 2) text.Color = Color.DarkGreen; });

      texts[2] = new Text(fontMedium, "Level 3", level3Pos,
        () => { Tilemap.Level = 3; },
        (Text text) => { if (Tilemap.Level == 3) text.Color = Color.DarkGreen; });

      texts[3] = new Text(fontMedium, "Level 4", level4Pos,
        () => { Tilemap.Level = 4; },
        (Text text) => { if (Tilemap.Level == 4) text.Color = Color.DarkGreen; });

      texts[4] = new Text(fontMedium, "Back", backPos, () => { ScreenManager.GoBack(1); });
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

    private static void checkLevelSelected(Text text) {

    }
  }
}