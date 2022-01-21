namespace QuoridorConsole
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int xValue, int yValue)
        {
            x = xValue;
            y = yValue;
        }


        public bool IsEqual(Vector2Int another)
        {
            if (another.x == x && another.y == y)
                return true;

            return false;
        }

        public static Vector2Int zero { get => new Vector2Int(0, 0); }
    }
}
