using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class MoveGridPart
    {
        private Vector2Int _gridPos;

        private bool _isWithPawn;

        private List<Direction> _availableDirection;

        private int _cost = 1;

        public int Cost
        {
            get => _cost;
            set => _cost = value;
        }

        public event Move OnMoveTo;
        public delegate void Move(MoveGridPart moveGridPart);


        public MoveGridPart(Vector2Int gridPos)
        {
            _gridPos = gridPos;

            OnStart();
        }


        public MoveGridPart()
        {

        }


        public void OnStart()
        {
            _availableDirection = new List<Direction>();

            if (_gridPos.x != 9)
                _availableDirection.Add(Direction.Right);

            if (_gridPos.x != 1)
                _availableDirection.Add(Direction.Left);

            if (_gridPos.y != 9)
                _availableDirection.Add(Direction.Top);

            if (_gridPos.y != 1)
                _availableDirection.Add(Direction.Bottom);
        }


        public void BlockDirection(Direction direction)
        {
            if (_availableDirection.Contains(direction))
                _availableDirection.Remove(direction);
        }


        public void UnblockDirection(Direction direction)
        {
            if (!_availableDirection.Contains(direction))
                _availableDirection.Add(direction);
        }


        public bool IsDirectionAvailable(Direction direction)
        {
            if (_availableDirection.Contains(direction))
                return true;

            return false;
        }


        public Vector2Int GridPos { get => _gridPos; }

        public bool IsWithPawn { get => _isWithPawn; set => _isWithPawn = value; }
    }
}
