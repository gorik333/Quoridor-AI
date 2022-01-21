using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class GameController
    {
        private const int PLAYER_COUNT = 1;

        private GridController _grid;

        private int _currentPlayer;


        public void OnStart(bool isFirstMove)
        {
            GridController gridController = new GridController();

            gridController.OnStart();

            _grid = gridController;

            gridController.OnMove += PlayerMoved;

            SpawnPawns(isFirstMove);

            MoveOrder();
        }


        private void SpawnPawns(bool isFirstMove)
        {
            _grid.SpawnPawn(isFirstMove);
            _grid.SpawnPawn(!isFirstMove);
        }


        private void PlayerMoved()
        {
            MoveOrder();
        }


        private void MoveOrder()
        {
            if (_currentPlayer > PLAYER_COUNT)
                _currentPlayer = 0;

            _grid.PawnMoveQueue(_currentPlayer++);
        }


        public GridController CurrentGrid { get => _grid; }
    }
}
