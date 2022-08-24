using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace snake {
  public static class InputManager {
    private static readonly int bufferLimit = 20;
    private static Queue<Keys> buffer;
    private static Keys? lastKey = null;
    private static MouseState lastMouseState = new();
    private static bool hasClicked = false; // true if click detected in update frame

    public static bool HasClickedAnywhere() {
      var windowSize = ScreenManager.Dimensions;
      return HasClickedOnArea(new Vector2(0, 0), windowSize);
    }

    public static bool HasClickedOnArea(Vector2 origin, Vector2 size) {
      return hasClicked && HasHoveredOverArea(origin, size);
    }

    // should be called once per update
    public static void CheckClick() {
      hasClicked = false;
      MouseState mouseState = Mouse.GetState();
      if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed) {
        hasClicked = true;
      }
      lastMouseState = mouseState;
    }

    public static bool HasHoveredOverArea(Vector2 origin, Vector2 size) {
      MouseState mouseState = Mouse.GetState();
      float x = mouseState.X;
      float y = mouseState.Y;
      return x >= origin.X && x <= origin.X + size.X && y >= origin.Y && y <= origin.Y + size.Y;
    }

    public static void CheckKeyPress() {
      KeyboardState keyboardState = Keyboard.GetState();

      if (keyboardState.IsKeyDown(Keys.W)) {
        AddKeyToBuffer(Keys.W);
        return;
      }

      if (keyboardState.IsKeyDown(Keys.A)) {
        AddKeyToBuffer(Keys.A);
        return;
      }

      if (keyboardState.IsKeyDown(Keys.S)) {
        AddKeyToBuffer(Keys.S);
        return;
      }

      if (keyboardState.IsKeyDown(Keys.D)) {
        AddKeyToBuffer(Keys.D);
        return;
      }
    }

    public static Keys GetKeyFromBuffer() {
      return buffer.Dequeue();
    }

    public static int BufferCount() {
      return buffer.Count;
    }

    public static void Initialize() {
      buffer = new ();
      lastKey = null;
    }

    #region helper functions
    private static void AddKeyToBuffer(Keys key) {
      if (buffer.Count < bufferLimit && lastKey != key) {
        buffer.Enqueue(key);
        lastKey = key;
      }
    }
    #endregion
  }
}
