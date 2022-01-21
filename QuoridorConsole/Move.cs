using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public struct Move
    {
        public Vector2Int Pos;
        public Direction MoveDirection;

        public Move(Vector2Int pos, Direction direction)
        {
            Pos = pos;
            MoveDirection = direction;
        }
    }
}
