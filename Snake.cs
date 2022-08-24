using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace snake {
  public class Snake {
    private readonly LinkedList<SnakePart> snakeParts = new();
    private readonly Texture2D[] textures;
    private Point velocity;
    private enum Direction { UP, LEFT, DOWN, RIGHT }

    public int Score { get; set; }
    public bool HasWon { get; set; }
    public bool IsDead { get; set; }
    public enum Texture { BODY_HORIZONTAL, BODY_VERTICAL, HEAD_UP, HEAD_LEFT, HEAD_DOWN, HEAD_RIGHT, TAIL_UP, TAIL_DOWN, TAIL_LEFT, TAIL_RIGHT, BODY_NODE1, BODY_NODE2, BODY_NODE3, BODY_NODE4 }

    public Snake(int initSize, Point initVelocity, Point initPoint, ContentManager content) {
      // loading textures
      textures = new Texture2D[14];
      textures[(int)Texture.BODY_HORIZONTAL] = LoadTexture(content, "body_horizontal");
      textures[(int)Texture.BODY_VERTICAL] = LoadTexture(content, "body_vertical");
      textures[(int)Texture.HEAD_UP] = LoadTexture(content, "head_up");
      textures[(int)Texture.HEAD_LEFT] = LoadTexture(content, "head_left");
      textures[(int)Texture.HEAD_DOWN] = LoadTexture(content, "head_down");
      textures[(int)Texture.HEAD_RIGHT] = LoadTexture(content, "head_right");
      textures[(int)Texture.TAIL_UP] = LoadTexture(content, "tail_up");
      textures[(int)Texture.TAIL_LEFT] = LoadTexture(content, "tail_left");
      textures[(int)Texture.TAIL_DOWN] = LoadTexture(content, "tail_down");
      textures[(int)Texture.TAIL_RIGHT] = LoadTexture(content, "tail_right");
      textures[(int)Texture.BODY_NODE1] = LoadTexture(content, "body_node1");
      textures[(int)Texture.BODY_NODE2] = LoadTexture(content, "body_node2");
      textures[(int)Texture.BODY_NODE3] = LoadTexture(content, "body_node3");
      textures[(int)Texture.BODY_NODE4] = LoadTexture(content, "body_node4");

      // constructing initial snake
      for (int i = 0; i < initSize; i++) {
        int x = initPoint.X - i;
        int y = initPoint.Y;
        Point point = new(x, y);
        if (i == 0) {
          SnakePart headPart = new(GetTexture(Texture.HEAD_RIGHT), point, SnakePart.Type.HEAD);
          AddFirst(headPart);
          continue;
        }
        if (i == initSize - 1) {
          SnakePart tailPart = new(GetTexture(Texture.TAIL_RIGHT), point, SnakePart.Type.BODY);
          AddLast(tailPart);
          continue;
        }
        SnakePart bodyPart = new(GetTexture(Texture.BODY_HORIZONTAL), point, SnakePart.Type.TAIL);
        AddLast(bodyPart);
      }

      HasWon = false;
      IsDead = false;
      velocity = initVelocity;
    }

    public void Draw(SpriteBatch spriteBatch) {
      foreach (SnakePart snakePart in snakeParts) {
        snakePart.Draw(spriteBatch);
      }
    }

    public void Update(Tilemap tilemap, Food food) {
      if (IsDead || HasWon) return;
      SetDirection();
      Point prevHeadPoint = MoveHead();

      if (HasCollided(tilemap)) {
        SoundManager.Play(SoundManager.Sound.DIE);
        IsDead = true;
        UndoMoveHead(prevHeadPoint);
        return;
      }

      if (HasEaten(food)) {
        // add a new snake part in prev head point (which is empty currently) 
        Grow(prevHeadPoint);
        Score += 1;
        var freePoints = Tilemap.GetFreePoints(tilemap.Spaces, GetSnakeParts());
        bool hasSpawnedFood = food.SpawnIn(freePoints);
        if (hasSpawnedFood) {
          SoundManager.Play(SoundManager.Sound.EAT);
        } else {
          SoundManager.Play(SoundManager.Sound.WIN);
          HasWon = true;
        }
      } else {
        MoveBodyAndTail(prevHeadPoint);
      }
      SetTextures();
    }

    public List<SnakePart> GetSnakeParts() {
      List<SnakePart> list = new();
      foreach (SnakePart snakePart in snakeParts) {
        list.Add(snakePart);
      }
      return list;
    }

    #region helper functions
    private void Grow(Point prevHeadPoint) {
      AddAfterHead(new SnakePart(textures[(int)Texture.BODY_HORIZONTAL], prevHeadPoint, SnakePart.Type.BODY));
    }

    // returns previous tail point
    private Point MoveBodyAndTail(Point prevHeadPoint) {
      Point prevPoint = prevHeadPoint;

      // first body
      var currentNode = snakeParts.First.Next;
      while (currentNode != null) {
        Point tempPoint = currentNode.Value.Point;
        currentNode.Value.Point = prevPoint;
        prevPoint = tempPoint;
        currentNode = currentNode.Next;
      }

      return prevPoint;
    }

    // returns previous head point
    private Point MoveHead() {
      SnakePart head = snakeParts.First.Value;
      Point prevPoint = head.Point;
      int x = Tilemap.Modulo(head.Point.X + velocity.X, Tilemap.size.X);
      int y = Tilemap.Modulo(head.Point.Y + velocity.Y, Tilemap.size.Y);
      head.Point = new Point(x, y);
      return prevPoint;
    }

    private void UndoMoveHead(Point prevHeadPoint) {
      SnakePart head = snakeParts.First.Value;
      head.Point = prevHeadPoint;
    }

    private bool HasEaten(Food food) {
      SnakePart head = snakeParts.First.Value;
      return head.Point.Equals(food.Point);
    }

    private bool HasCollided(Tilemap tilemap) {
      var walls = tilemap.Walls;
      foreach (var wall in walls) {
        if (wall.Contains(snakeParts.First.Value.Point)) return true;
      }

      var head = snakeParts.First.Value;
      foreach (var snakePart in snakeParts) {
        if (snakePart.type == SnakePart.Type.HEAD) continue;
        if (snakePart.Contains(head.Point)) return true;
      }

      return false;
    }

    private void SetDirection() {
      if (InputManager.BufferCount() == 0) return;
      Keys key = InputManager.GetKeyFromBuffer();

      switch (key) {
        case (Keys.W):
          velocity = velocity.X == 0 ? velocity : (new Point(0, -1));
          break;
        case (Keys.A):
          velocity = velocity.Y == 0 ? velocity : new Point(-1, 0);
          break;
        case (Keys.S):
          velocity = velocity.X == 0 ? velocity : new Point(0, 1);
          break;
        case (Keys.D):
          velocity = velocity.Y == 0 ? velocity : new Point(1, 0);
          break;
        default:
          break;
      }
    }

    private void AddFirst(SnakePart snakePart) {
      snakeParts.AddFirst(new LinkedListNode<SnakePart>(snakePart));
    }

    private void AddAfterHead(SnakePart snakePart) {
      snakeParts.AddAfter(snakeParts.First, new LinkedListNode<SnakePart>(snakePart));
    }

    private void AddLast(SnakePart snakePart) {
      snakeParts.AddLast(new LinkedListNode<SnakePart>(snakePart));
    }

    private Texture2D GetTexture(Texture texture) {
      return textures[(int)texture];
    }

    private void SetTextures() {
      var currentNode = snakeParts.First;

      while (currentNode != null) {
        var nextNode = currentNode.Next;
        var prevNode = currentNode.Previous;

        if (prevNode == null && nextNode != null) {
          Direction nextDirection = GetSnakePartDirection(currentNode.Value, nextNode.Value);
          SetHeadTexture(currentNode.Value, nextDirection);
          currentNode = currentNode.Next;
          continue;
        }

        if (prevNode != null && nextNode != null) {
          Direction prevDirection = GetSnakePartDirection(currentNode.Value, prevNode.Value);
          Direction nextDirection = GetSnakePartDirection(currentNode.Value, nextNode.Value);
          SetBodyTexture(currentNode.Value, prevDirection, nextDirection);
          currentNode = currentNode.Next;
          continue;
        }

        if (prevNode != null && nextNode == null) {
          Direction prevDirection = GetSnakePartDirection(currentNode.Value, prevNode.Value);
          SetTailTexture(currentNode.Value, prevDirection);
          currentNode = currentNode.Next;
          continue;
        }
      }
    }

    private void SetHeadTexture(SnakePart head, Direction nextDirection) {
      switch (nextDirection) {
        case Direction.UP:
          head.Texture = GetTexture(Texture.HEAD_DOWN);
          break;
        case Direction.LEFT:
          head.Texture = GetTexture(Texture.HEAD_RIGHT);
          break;
        case Direction.DOWN:
          head.Texture = GetTexture(Texture.HEAD_UP);
          break;
        case Direction.RIGHT:
          head.Texture = GetTexture(Texture.HEAD_LEFT);
          break;
      }
    }

    private void SetBodyTexture(SnakePart body, Direction prevDirection, Direction nextDirection) {
      if ((prevDirection == Direction.UP && nextDirection == Direction.DOWN) || (prevDirection == Direction.DOWN && nextDirection == Direction.UP)) {
        body.Texture = GetTexture(Texture.BODY_VERTICAL);
        return;
      }

      if ((prevDirection == Direction.LEFT && nextDirection == Direction.RIGHT) || (prevDirection == Direction.RIGHT && nextDirection == Direction.LEFT)) {
        body.Texture = GetTexture(Texture.BODY_HORIZONTAL);
        return;
      }

      if ((prevDirection == Direction.UP && nextDirection == Direction.RIGHT) || (prevDirection == Direction.RIGHT && nextDirection == Direction.UP)) {
        body.Texture = GetTexture(Texture.BODY_NODE1);
        return;
      }

      if ((prevDirection == Direction.UP && nextDirection == Direction.LEFT) || (prevDirection == Direction.LEFT && nextDirection == Direction.UP)) {
        body.Texture = GetTexture(Texture.BODY_NODE2);
        return;
      }

      if ((prevDirection == Direction.DOWN && nextDirection == Direction.LEFT) || (prevDirection == Direction.LEFT && nextDirection == Direction.DOWN)) {
        body.Texture = GetTexture(Texture.BODY_NODE3);
        return;
      }

      if ((prevDirection == Direction.DOWN && nextDirection == Direction.RIGHT) || (prevDirection == Direction.RIGHT && nextDirection == Direction.DOWN)) {
        body.Texture = GetTexture(Texture.BODY_NODE4);
        return;
      }
    }

    private void SetTailTexture(SnakePart tail, Direction prevDirection) {
      switch (prevDirection) {
        case Direction.UP:
          tail.Texture = GetTexture(Texture.TAIL_UP);
          break;
        case Direction.LEFT:
          tail.Texture = GetTexture(Texture.TAIL_LEFT);
          break;
        case Direction.DOWN:
          tail.Texture = GetTexture(Texture.TAIL_DOWN);
          break;
        case Direction.RIGHT:
          tail.Texture = GetTexture(Texture.TAIL_RIGHT);
          break;
      }
    }
  #endregion

    #region static variables
    private static Texture2D LoadTexture(ContentManager content, string name) {
      return content.Load<Texture2D>("sprites\\snake\\" + name);
    }

    private static Direction GetSnakePartDirection(SnakePart current, SnakePart other) {
      int currentX = current.Point.X;
      int currentY = current.Point.Y;
      int otherX = other.Point.X;
      int otherY = other.Point.Y;
      
      if (currentX == otherX)
        return (Tilemap.Modulo(currentY - 1, Tilemap.size.Y)) == otherY ? Direction.UP : Direction.DOWN;
      else
        return (Tilemap.Modulo(currentX - 1, Tilemap.size.X)) == otherX ? Direction.LEFT : Direction.RIGHT;
    }
    #endregion
  }
}
