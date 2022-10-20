using System;

namespace GameCommon
{
    [System.Serializable]
    public class Vector2I
    {
        public int x;
        public int y;
        public static Vector2I operator -(Vector2I left, Vector2I right)
        {
            Vector2I _temp = new Vector2I();
            _temp.x = left.x - right.x;
            _temp.y = left.y - right.y;
            return _temp;
        }
        public static bool operator ==(Vector2I left, Vector2I right)
        {
            return left.x == right.x && left.y == right.y;
        }
        public static bool operator !=(Vector2I left, Vector2I right)
        {
            return left.x != right.x || left.y != right.y;
        }
        public static int Distance(Vector2I left, Vector2I right)
        {
            return Math.Abs(left.x - right.x) + Math.Abs(left.y - right.y);
        }
        public override bool Equals(object other)
        {
            if (!(other is Vector2I))
            {
                return false;
            }
            return this == (Vector2I)other;
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}
