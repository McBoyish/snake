using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace snake {
  public static class ScreenManager {
    private static ContentManager content;
    private static Stack<Screen> screenStack;

    public static Vector2 Dimensions { get; set; }

    public static void AddScreen(Screen screen) {
      if (screenStack.Count > 0) {
        screenStack.Peek().UnloadContent();
      }
      screenStack.Push(screen);
      screenStack.Peek().Initialize();
      screenStack.Peek().LoadContent(content);
    }

    public static void GoBack(int numberOfScreens) {
      int numberOfScreensToRemove = numberOfScreens < screenStack.Count ? numberOfScreens : screenStack.Count - 1;
      for (int i = 0; i < numberOfScreensToRemove; ++i) {
        screenStack.Peek().UnloadContent();
        screenStack.Pop();
      }
      screenStack.Peek().Initialize();
      screenStack.Peek().LoadContent(content);
    }

    public static void Initialize() {
      Dimensions = new Vector2(800f, 640f);
      screenStack = new();
      Tilemap.Level = 1;
    }

    public static void LoadContent(ContentManager c) {
      content = c;
      AddScreen(new MainMenuScreen());
    }

    public static void Update(GameTime gameTime) {
      InputManager.CheckClick();
      screenStack.Peek().Update(gameTime);
    }

    public static void Draw(SpriteBatch spriteBatch) {
      screenStack.Peek().Draw(spriteBatch);
    }
  }
}
