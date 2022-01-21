using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class NextStep
    {
        private bool _isRun;
        private MoveGridPart _move;
        private Vector2Int _position;
        private bool _isVertical;

        public bool isRun 
        { 
            get => _isRun;
            set => _isRun = value;
        }

        public MoveGridPart Move
        {
            get => _move;
            set => _move = value;
        }

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        public bool isVertical
        {
            get => _isVertical;
            set => _isVertical = value;
        }


    }
}
