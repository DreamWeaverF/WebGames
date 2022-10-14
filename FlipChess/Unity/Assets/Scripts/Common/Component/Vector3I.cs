namespace GameCommon
{
    [System.Serializable]
    public class Vector3I
    {
        public int x;
        public int y;
        public int z;
        public static Vector3I operator -(Vector3I left, Vector3I right)
        {
            Vector3I _temp = new Vector3I();
            _temp.x = left.x - right.x;
            _temp.y = left.y - right.y;
            _temp.z = left.z - right.z;
            return _temp;
        }
        public static bool operator ==(Vector3I left, Vector3I right)
        {
            return left.x == right.x && left.y == right.y && left.z == right.z;
        }
        public static bool operator !=(Vector3I left, Vector3I right)
        {
            return left.x != right.x || left.y != right.y || left.z != right.z;
        }
        public override bool Equals(object other)
        {
            if (!(other is Vector3I))
            {
                return false;
            }
            return this == (Vector3I)other;
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}
