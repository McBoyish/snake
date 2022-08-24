using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace snake {
  public class PostGameScreen : Screen {
    private SpriteFont fontMedium;
    private SpriteFont fontLarge;
    private Text[] texts;
    private readonly string result;
    private readonly string score;

    public PostGameScreen(string result, string score) {
      this.result = result;
      this.score = score;
    }

    public override void Initialize() {
      base.Initialize();
      texts = new Text[2];
    }

    public override void LoadContent(ContentManager c) {
      base.LoadContent(c);

      fontLarge = content.Load<SpriteFont>("font\\retro_large");
      fontMedium = content.Load<SpriteFont>("font\\retro_medium");

      float width = ScreenManager.Dimensions.X;
      float height = ScreenManager.Dimensions.Y;

      Vector2 resultPosition = new(width / 2, 0.1f * height);
      Vector2 scorePosition = new(width / 2, 0.3f * height);

      texts[0] = new Text(fontLarge, result, resultPosition);
      texts[1] = new Text(fontMedium, score, scorePosition);
    }

    public override void UnloadContent() {
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime) {
      if (InputManager.HasClickedAnywhere()) {
        SoundManager.Play(SoundManager.Sound.SELECT);
        ScreenManager.GoBack(2);
      }
    }

    public override void Draw(SpriteBatch spriteBatch) {
      foreach (Text text in texts) {
        text.Draw(spriteBatch);
      }
    }
  }
}
