#define DEBUG

namespace snake {
  public static class Debug {
    public static void Assert(bool condition) {
      System.Diagnostics.Debug.Assert(condition);
    }

    public static void Write(string message) {
#if (DEBUG)
      System.Diagnostics.Debug.Write(message);
#endif
    }

    public static void WriteLine(string message) {
#if (DEBUG)
      System.Diagnostics.Debug.WriteLine(message);
#endif
    }
  }
}
