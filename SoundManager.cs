using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace snake {
  public static class SoundManager {
    public static SoundEffect[] soundEffects;
    public enum Sound { EAT, WIN, DIE, SELECT }

    public static void LoadContent(ContentManager content) {
      soundEffects = new SoundEffect[4];
      soundEffects[(int)Sound.EAT] = LoadSound(content, "eat");
      soundEffects[(int)Sound.WIN] = LoadSound(content, "win");
      soundEffects[(int)Sound.DIE] = LoadSound(content, "die");
      soundEffects[(int)Sound.SELECT] = LoadSound(content, "select");
    }

    public static void Play(Sound sound) {
      soundEffects[(int)sound].Play();
    }

    #region helper functions
    private static SoundEffect LoadSound(ContentManager content, string name) {
      return content.Load<SoundEffect>("sounds\\" + name);
    }
    #endregion
  }
}
