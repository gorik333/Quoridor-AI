using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class View
    {
        private Dictionary<char, int> _moveToInt = new Dictionary<char, int>();
        private Dictionary<char, int> _wallToInt = new Dictionary<char, int>();

        private GameController _current;

        private const string MOVE_COMMAND = "move";
        private const string JUMP_COMMAND = "jump";
        private const string WALL_COMMAND = "wall";

        private const char VERTICAL = 'v';
        private const char HORIZONTAL = 'h';

        public void Start()
        {
            InitializeDictionary();

            string color = Console.ReadLine();

            GameController gameController = new GameController();

            if (color == "white")
                gameController.OnStart(false);
            if (color == "black")
                gameController.OnStart(true);

            _current = gameController;


            while (true)
            {
                string move = Console.ReadLine();

                string[] commandAndValue = move.Split();

                switch (commandAndValue[0])
                {
                    case MOVE_COMMAND:
                        MoveProcess(commandAndValue[1]);

                        break;
                    case WALL_COMMAND:
                        WallProcess(commandAndValue[1]);

                        break;
                    case JUMP_COMMAND:
                        JumpProcess(commandAndValue[1]);

                        break;
                }
            }

        }


        private void MoveProcess(string input)
        {
            Vector2Int movePos = Vector2Int.zero;

            char[] symbols = input.ToCharArray();

            int x = 0;
            int y = 0;

            if (_moveToInt.TryGetValue(symbols[0], out int value))
                x = value;

            movePos.x = x;

            y = int.Parse(symbols[1].ToString());

            movePos.y = y;

            _current.CurrentGrid.PlayerMove(movePos);
        }


        private void WallProcess(string input)
        {
            Vector2Int wallPos = Vector2Int.zero;

            char[] symbols = input.ToCharArray();

            int x = 0;
            int y = 0;

            if (_wallToInt.TryGetValue(symbols[0], out int value))
                x = value;

            wallPos.x = x;

            y = int.Parse(symbols[1].ToString());

            wallPos.y = y;

            bool isVertical = false;

            switch (symbols[2])
            {
                case HORIZONTAL:
                    isVertical = false;

                    break;
                case VERTICAL:
                    isVertical = true;

                    break;
            }

            _current.CurrentGrid.PlaceWall(wallPos, isVertical);
        }


        private void JumpProcess(string input)
        {
            Vector2Int jumpPos = Vector2Int.zero;

            char[] symbols = input.ToCharArray();

            int x = 0;
            int y = 0;

            if (_moveToInt.TryGetValue(symbols[0], out int value))
                x = value;

            jumpPos.x = x;

            y = int.Parse(symbols[1].ToString());

            jumpPos.y = y;

            _current.CurrentGrid.PlayerMove(jumpPos);
        }


        private void InitializeDictionary()
        {
            _moveToInt.Add('A', 1);
            _moveToInt.Add('B', 2);
            _moveToInt.Add('C', 3);
            _moveToInt.Add('D', 4);
            _moveToInt.Add('E', 5);
            _moveToInt.Add('F', 6);
            _moveToInt.Add('G', 7);
            _moveToInt.Add('H', 8);
            _moveToInt.Add('I', 9);

            _wallToInt.Add('S', 1);
            _wallToInt.Add('T', 2);
            _wallToInt.Add('U', 3);
            _wallToInt.Add('V', 4);
            _wallToInt.Add('W', 5);
            _wallToInt.Add('X', 6);
            _wallToInt.Add('Y', 7);
            _wallToInt.Add('Z', 8);
        }
    }
}
