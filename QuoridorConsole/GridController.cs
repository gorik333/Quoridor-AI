using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class GridController
    {
        private List<Pawn> _pawn;
        private List<Vector2Int> _spawnPos;
        private List<MoveGridPart> _moveGridPart;
        private List<WallGridPart> _wallGridPart;

        private Pawn _currentPawn;

        private int _pawnCount;

        private Random random = new Random();

        public List<Pawn> CurrentPawn { get => _pawn; set => _pawn = value; }

        public List<MoveGridPart> MoveGridPart { get => _moveGridPart; set => _moveGridPart = value; }

        public List<WallGridPart> WallGridPart { get => _wallGridPart; set => _wallGridPart = value; }

        public event Move OnMove;
        public delegate void Move();


        public void OnStart()
        {
            InitializeMoveGridParts(); // 1
            InitializeWallGripParts(); // 2
            InitializePawnSpawnPos(); // 3
            InitializePawnList();
        }


        public void InitializeWallGripParts()
        {
            _wallGridPart = new List<WallGridPart>();

            for (int i = 8; i > 0; i--)
            {
                for (int j = 1; j < 9; j++)
                {
                    _wallGridPart.Add(new WallGridPart(new Vector2Int(j, i), true, true)); // j = X, i = Y
                }
            }

            for (int i = 0; i < _wallGridPart.Count; i++)
            {
                _wallGridPart[i].OnDelete += UnblockNearMoveGrid;
            }
        }


        public void InitializeMoveGridParts()
        {
            _moveGridPart = new List<MoveGridPart>();

            for (int i = 9; i > 0; i--)
            {
                for (int j = 1; j < 10; j++)
                {
                    _moveGridPart.Add(new MoveGridPart(new Vector2Int(j, i))); // j = X, i = Y
                }
            }

            for (int i = 0; i < _moveGridPart.Count; i++)
            {
                _moveGridPart[i].OnMoveTo += MoveToGridPart;
            }
        }


        private void InitializePawnSpawnPos()
        {
            _spawnPos = new List<Vector2Int>();

            _spawnPos.Add(new Vector2Int(5, 9)); // BOTTOM CENTER GRID PART  5 = W, 1 = H  
            _spawnPos.Add(new Vector2Int(5, 1)); // TOP CENTER GRID PART     5 = W, 9 = H
        }


        public void PlaceWall(Vector2Int pos, bool isVertical, bool isMove = true)
        {
            var wallGridPart = GetWallPart(pos);

            wallGridPart.PlaceWall(isVertical);

            BlockNearMoveGrid(wallGridPart.GridPos, isVertical);
            BlockNearWallAvailablePlace(wallGridPart.GridPos, isVertical);

            if (isMove)
                OnMove?.Invoke();
        }


        private void BlockNearMoveGrid(Vector2Int wallPos, bool isVertical)
        {
            var blockedWays = new List<GridParam>();

            if (isVertical)
            {
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Right));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Right));

                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Left));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Left));
            }
            else
            {
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Bottom));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Top));

                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Bottom));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Top));
            }

            for (int i = 0; i < _moveGridPart.Count; i++)
            {
                for (int j = 0; j < blockedWays.Count; j++)
                {
                    if (_moveGridPart[i].GridPos.IsEqual(blockedWays[j].GridPos))
                    {
                        _moveGridPart[i].BlockDirection(blockedWays[j].GridDirection);

                        break;
                    }
                }
            }
        }


        private void UnblockNearMoveGrid(WallGridPart wall, bool isVertical)
        {
            var wallPos = wall.GridPos;

            var blockedWays = new List<GridParam>();

            if (isVertical)
            {
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Right));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Right));

                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Left));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Left));
            }
            else
            {
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Bottom));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Top));

                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Bottom));
                blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Top));
            }

            for (int i = 0; i < _moveGridPart.Count; i++)
            {
                for (int j = 0; j < blockedWays.Count; j++)
                {
                    if (_moveGridPart[i].GridPos.IsEqual(blockedWays[j].GridPos))
                    {
                        _moveGridPart[i].UnblockDirection(blockedWays[j].GridDirection);

                        break;
                    }
                }
            }
        }


        private void BlockNearWallAvailablePlace(Vector2Int wallPos, bool isVertical)
        {
            var blockPlaces = new List<Vector2Int>();

            if (isVertical)
            {
                blockPlaces.Add(new Vector2Int(wallPos.x, wallPos.y - 1));
                blockPlaces.Add(new Vector2Int(wallPos.x, wallPos.y + 1));
            }
            else
            {
                blockPlaces.Add(new Vector2Int(wallPos.x + 1, wallPos.y));
                blockPlaces.Add(new Vector2Int(wallPos.x - 1, wallPos.y));
            }

            for (int i = 0; i < _wallGridPart.Count; i++)
            {
                for (int j = 0; j < blockPlaces.Count; j++)
                {
                    if (_wallGridPart[i].GridPos.IsEqual(blockPlaces[j]))
                    {
                        _wallGridPart[i].DisallowPlacement(isVertical);

                        break;
                    }
                }
            }
        }


        private void InitializePawnList()
        {
            _pawn = new List<Pawn>();
        }


        public void SpawnPawn(bool isPlayer)
        {
            var spawnPos = _spawnPos[_pawnCount];
            var pawn = new Pawn(spawnPos, isPlayer);
            var gridPart = GetMovePart(spawnPos);

            gridPart.IsWithPawn = true;
            _pawn.Add(pawn);
            _pawnCount++;
        }


        public void MoveToGridPart(MoveGridPart nextGridPart)
        {
            _currentPawn.MoveToPart(nextGridPart);

            nextGridPart.IsWithPawn = true;

            OnMove?.Invoke();
        }


        public void PawnMoveQueue(int pawnIndex)
        {
            _currentPawn = _pawn[pawnIndex];

            if (!_currentPawn.IsPlayer)
                ComputerMove(_currentPawn);
        }


        public void PlayerMove(Vector2Int movePos)
        {
            var nextMoveGrid = GetMovePart(movePos);
            var currentMoveGrid = GetMovePart(_currentPawn.PawnPos);

            currentMoveGrid.IsWithPawn = false;

            MoveToGridPart(nextMoveGrid);
        }


        private void ComputerMove(Pawn pawn)
        {
            MoveGridPart currentMovePart = GetMovePart(pawn.PawnPos);

            var isMove = true;

            var isVerticalWall = (float)random.NextDouble() > 0.5f;

            if (pawn.VerticalWallPlaced >= 10 && isVerticalWall)
                isMove = true;
            else if (pawn.VerticalWallPlaced < 10 && isVerticalWall)
                pawn.VerticalWallPlaced++;


            if (pawn.HorizontalWallPlaced >= 10 && !isVerticalWall)
                isMove = true;
            else if (pawn.HorizontalWallPlaced < 10 && !isVerticalWall)
                pawn.HorizontalWallPlaced++;


            if (!isMove)
            {
                var wall = GetRandomWallGridPart(isVerticalWall);

                var ai = new AI();
                var pathFinding = new Pathfinding();

                if (wall != null)
                {
                    var copy = GridControllerCopy();

                    copy.PlaceWall(wall.GridPos, isVerticalWall, false);

                    MoveGridPart playerGrid = copy.MoveGridPart.FindAll(e => e.IsWithPawn && !e.GridPos.IsEqual(currentMovePart.GridPos)).FirstOrDefault();

                    var playerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos.IsEqual(playerGrid.GridPos)).FirstOrDefault();
                    var computerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos.IsEqual(currentMovePart.GridPos)).FirstOrDefault();

                    var nextMoveEnemy = ai.RunMove(currentMovePart, copy.MoveGridPart, pathFinding, false, playerPawn.StartY);
                    var nextMovePlayer = ai.RunMove(playerGrid, copy.MoveGridPart, pathFinding, true, computerPawn.StartY);

                    if (nextMoveEnemy != null && nextMovePlayer != null)
                    {
                        if (nextMoveEnemy.Count != 0 && nextMovePlayer.Count != 0)
                        {
                            wall.PlaceWall(isVerticalWall);
                        }
                        else
                        {
                            isMove = true;
                        }
                    }
                }
                else
                    isMove = true;
            }

            if (isMove)
            {
                currentMovePart.IsWithPawn = false;

                var ai = new AI();

                GridController controllerCopy = GridControllerCopy();

                Pawn _opponentPawn = CurrentPawn.Find(p => p != _currentPawn);

                var move = ai.NextMove(controllerCopy, currentMovePart, _moveGridPart, _currentPawn, _opponentPawn, false);


                if(!move.isRun)
                {
                    PlaceWall(move.Position, move.isVertical);
                    //Console.WriteLine("From placeWall: " + move.isVertical.ToString() + " " + move.Position.x.ToString() + move.Position.y.ToString());
                }
                else
                {
                    MoveToGridPart(move.Move);
                    //var wall = ai.BlockOpponent(controllerCopy);
                }



                if (move.isRun)
                {
                    int x = Math.Abs(currentMovePart.GridPos.x - move.Move.GridPos.x);
                    int y = Math.Abs(currentMovePart.GridPos.y - move.Move.GridPos.y);

                    if (y == 2 || x == 2)
                    {
                        VectorToString("jump", move.Move.GridPos.x, move.Move.GridPos.y);
                    }
                    if (y != 2 && x != 2)
                    {
                        VectorToString("move", move.Move.GridPos.x, move.Move.GridPos.y);
                    }
                }
                else
                {
                    WallToString(move.Position.x, move.Position.y, move.isVertical);

                    //Console.WriteLine("Wall placed on" + move.Position.x + ", " + move.Position.y );
                    //Console.WriteLine("Computer Pawn Pos " + currentMovePart.GridPos.x + ", " + currentMovePart.GridPos.y);
                }

                //if x = 0 && y = 0 => VectorToString("wall", ..., ...);
            }
        }


        private void WallToString(int x, int y, bool isVertical)
        {
            Dictionary<int, char> intToWall = new Dictionary<int, char>();
            intToWall.Add(1, 'S');
            intToWall.Add(2, 'T');
            intToWall.Add(3, 'U');
            intToWall.Add(4, 'V');
            intToWall.Add(5, 'W');
            intToWall.Add(6, 'X');
            intToWall.Add(7, 'Y');
            intToWall.Add(8, 'Z');

            string result = "wall";
            string wallOrientation;
            if (isVertical)
                wallOrientation = "v";
            else
                wallOrientation = "h";


            if (intToWall.TryGetValue(x, out char value))
            {
                var xLetter = value.ToString();

                result += " " + xLetter.ToString() + y.ToString() + wallOrientation;
            }

            Console.WriteLine(result);
        }

        private void VectorToString(string moveOrJump, int x, int y)
        {
            Dictionary<int, char> dictionary = new Dictionary<int, char>();

            dictionary.Add(1, 'A');
            dictionary.Add(2, 'B');
            dictionary.Add(3, 'C');
            dictionary.Add(4, 'D');
            dictionary.Add(5, 'E');
            dictionary.Add(6, 'F');
            dictionary.Add(7, 'G');
            dictionary.Add(8, 'H');
            dictionary.Add(9, 'I');

            string result = moveOrJump;

            if (dictionary.TryGetValue(x, out char value))
            {
                var xLetter = value.ToString();

                result += " " + xLetter.ToString() + y.ToString();
            }

            Console.WriteLine(result);
        }


        private GridController GridControllerCopy()
        {
            GridController copy = (GridController)this.MemberwiseClone();

            copy.MoveGridPart = new List<MoveGridPart>(_moveGridPart);
            copy.WallGridPart = new List<WallGridPart>(_wallGridPart);
            copy.CurrentPawn = _pawn;

            return copy;
        }


        private MoveGridPart GetMovePart(Vector2Int pos)
        {
            for (int i = 0; i < _moveGridPart.Count; i++)
            {
                if (_moveGridPart[i].GridPos.IsEqual(pos))
                {
                    return _moveGridPart[i];
                }
            }

            return null;
        }


        private WallGridPart GetWallPart(Vector2Int pos)
        {
            for (int i = 0; i < _wallGridPart.Count; i++)
            {
                if (_wallGridPart[i].GridPos.IsEqual(pos))
                {
                    return _wallGridPart[i];
                }
            }

            return null;
        }


        private WallGridPart GetRandomWallGridPart(bool isVertical)
        {
            var wallGridParts = new List<WallGridPart>();

            for (int i = 0; i < _wallGridPart.Count; i++)
            {
                if (_wallGridPart[i].IsHorizontalAllow && !isVertical)
                {
                    if (!wallGridParts.Contains(_wallGridPart[i]))
                        wallGridParts.Add(_wallGridPart[i]);
                }
                else if (_wallGridPart[i].IsVerticalAllow && isVertical)
                {
                    if (!wallGridParts.Contains(_wallGridPart[i]))
                        wallGridParts.Add(_wallGridPart[i]);
                }
            }

            if (wallGridParts.Count > 0)
            {
                var rand = random.Next(0, wallGridParts.Count);

                var result = wallGridParts[rand];

                return result;
            }
            else
                return null;
        }
    }

}
