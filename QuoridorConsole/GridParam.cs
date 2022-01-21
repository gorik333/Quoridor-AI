using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public struct GridParam
    {
        public Vector2Int GridPos;
        public Direction GridDirection;

        public GridParam(Vector2Int gridPos, Direction direction)
        {
            GridPos = gridPos;
            GridDirection = direction;
        }
    }
}
