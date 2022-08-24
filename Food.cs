using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace snake {
  public class Food : GameObject {
    private readonly Random random;

    public Food(Texture2D texture, List<Point> freePoints) : base(texture) {
      random = new Random();
      SpawnIn(freePoints);
    }

    public bool SpawnIn(List<Point> freePoints) {
      if (freePoints.Count == 0) return false;
      int randomIndex = random.Next(freePoints.Count);
      Point = freePoints[randomIndex];
      return true;
    }
  }
}
