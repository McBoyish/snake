using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace snake {
  public class GameScreen : Screen {
    private Tilemap tilemap;
    private Snake snake;
    private Food food;
    private float time;

    public override void Initialize() {
      base.Initialize();
      time = 0f;
    }

    public override void LoadContent(ContentManager c) {
      base.LoadContent(c);

      Texture2D foodTexture = content.Load<Texture2D>("sprites\\food");

      tilemap = new Tilemap(content);
      snake = new Snake(3, new Point(1, 0), new Point(18, 18), content);

      List<Point> freePoints = Tilemap.GetFreePoints(tilemap.Spaces, snake.GetSnakeParts());
      food = new Food(foodTexture, freePoints);
    }

    public override void UnloadContent() {
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime) {
      time += gameTime.ElapsedGameTime.Milliseconds;
      bool HasGameEnded = snake.HasWon || snake.IsDead;

      if (!HasGameEnded) {
        InputManager.CheckKeyPress();
        if (time > timeStep) {
          time = 0f;
          snake.Update(tilemap, food);
        }
      }

      if (HasGameEnded && time > pauseTime) {
        string result = snake.HasWon ? "YOU WON!" : "YOU DIED!";
        ScreenManager.AddScreen(new PostGameScreen(result, "Score: " + snake.Score));
      }
    }

    public override void Draw(SpriteBatch spriteBatch) {
      tilemap.Draw(spriteBatch);
      snake.Draw(spriteBatch);
      food.Draw(spriteBatch);
    }

    #region static variables
    private static readonly float timeStep = 135f;
    private static readonly float pauseTime = 500f;
    #endregion
  }
}
