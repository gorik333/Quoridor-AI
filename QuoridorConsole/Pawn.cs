using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class Pawn
    {
        private Vector2Int _currentPos;

        private int _startY;

        private int _verticalWallPlaced;
        private int _horizontalWallPlaced;

        private bool _isPlayer;


        public Pawn(Vector2Int pos, bool isPlayer)
        {
            _currentPos = pos;
            _startY = pos.y;
            _isPlayer = isPlayer;
        }

        
        public Pawn()
        {

        }


        public void OnStart()
        {
            _currentPos = Vector2Int.zero;
        }


        public void MoveToPart(MoveGridPart gridPart)
        {
            _currentPos = gridPart.GridPos;
        }


        public void SetUp(MoveGridPart gridPart, bool isPlayer)
        {
            _isPlayer = isPlayer;

            MoveToPart(gridPart);
        }


        public Vector2Int PawnPos { get => _currentPos; }

        public bool IsPlayer { get => _isPlayer; }

        public int StartY { get => _startY; set => _startY = value; }

        public int HorizontalWallPlaced { get => _horizontalWallPlaced; set => _horizontalWallPlaced = value; }

        public int VerticalWallPlaced { get => _verticalWallPlaced; set => _verticalWallPlaced = value; }
    }
}
