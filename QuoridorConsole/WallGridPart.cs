using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class WallGridPart
    {
        private Vector2Int _gridPos;

        private bool _isVerticalAllow;
        private bool _isHorizontalAllow;

        public event Delete OnDelete;
        public delegate void Delete(WallGridPart wallGridPart, bool isVertical);


        public WallGridPart(Vector2Int gridPos, bool isVerticalAllow, bool isHorizontalAllow)
        {
            _gridPos = gridPos;
            _isVerticalAllow = isVerticalAllow;
            _isHorizontalAllow = isHorizontalAllow;
        }


        public void PlaceWall(bool isVertical)
        {
            _isHorizontalAllow = false;
            _isVerticalAllow = false;
        }


        public void DisallowPlacement(bool isVertical)
        {
            if (isVertical)
                _isVerticalAllow = false;
            else
                _isHorizontalAllow = false;
        }


        public Vector2Int GridPos { get => _gridPos; }

        public bool IsVerticalAllow { get => _isVerticalAllow; }

        public bool IsHorizontalAllow { get => _isHorizontalAllow; }
    }
}
