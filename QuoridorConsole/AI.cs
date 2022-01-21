using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class AI
    {
        public List<List<MoveGridPart>> GetFieldStatesForRunMoves(MoveGridPart curPosition, List<MoveGridPart> curField)
        {
            List<MoveGridPart> neighbours = new List<MoveGridPart>();
            var move = PossibleMove.GetPossibleMoves(curField, curPosition);

            foreach (Vector2Int v in move)
            {
                MoveGridPart g = curField.Find(x => x.GridPos.x == v.x && x.GridPos.y == v.y);
                if (g != null)
                    neighbours.Add(g);
            }

            List<List<MoveGridPart>> list = new List<List<MoveGridPart>>();


            foreach (MoveGridPart possibleMove in neighbours)
            {
                List<MoveGridPart> copy = new List<MoveGridPart>(curField);

                copy.Find(x => x == curPosition).IsWithPawn = false;
                copy.Find(x => x == possibleMove).IsWithPawn = true;
                list.Add(copy);
            }


            return list;

        }


        #region


        MoveGridPart selectedMove = new MoveGridPart();
        List<MoveGridPart> selectedFieldState = new List<MoveGridPart>();

        public int minimax(MoveGridPart curPos, List<MoveGridPart> position, int depth, bool maximizingPlayer)
        {

            if (depth == 0)
            {
                return Evaluation(position, curPos, false);    //return 
            }

            if (maximizingPlayer)
            {
                int maxEval = Int16.MinValue;
                foreach (List<MoveGridPart> child in GetFieldStatesForRunMoves(curPos, position))                  //perhabs position is a field    //child of position - position that can be achieved by a single move. Moves + walls
                {
                    MoveGridPart childPawnPos = child.FindLast(p => p.IsWithPawn == true);
                    int eval = minimax(childPawnPos, child, depth - 1, false);
                    if (eval > maxEval)
                        selectedFieldState = child;      //do we need it?

                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                int minEval = Int16.MaxValue;
                foreach (List<MoveGridPart> child in GetFieldStatesForRunMoves(curPos, position))
                {
                    MoveGridPart childPawnPos = child.Find(p => p.IsWithPawn == true);
                    int eval = minimax(childPawnPos, child, depth - 1, true);
                    if (eval < minEval)
                        selectedFieldState = child;

                    minEval = Math.Min(minEval, eval);
                }

                return minEval;
            }


        }



        int Evaluation(List<MoveGridPart> field, MoveGridPart curPosition, bool isPlayerPawn)
        {
            Pathfinding dijkstra = new Pathfinding();

            return 0;
        }




        #endregion

        public GridController CopyGridController(GridController toCopy)
        {
            List<MoveGridPart> copyMoveGridPart = new List<MoveGridPart>(toCopy.MoveGridPart);
            List<WallGridPart> copyWallGridPart = new List<WallGridPart>(toCopy.WallGridPart);

            return new GridController() { };
        }

        private GridController GridControllerCopy(GridController controllerToCopy)
        {
            GridController copy = new GridController();
            //{
            //    MoveGridPart = new List<MoveGridPart>(controllerToCopy.MoveGridPart),
            //    WallGridPart = new List<WallGridPart>(controllerToCopy.WallGridPart)
            //};

            copy.MoveGridPart = new List<MoveGridPart>(controllerToCopy.MoveGridPart);
            copy.WallGridPart = new List<WallGridPart>(controllerToCopy.WallGridPart);
            copy.CurrentPawn = new List<Pawn>(controllerToCopy.CurrentPawn);

            return copy;
        }




        public NextStep TryBlockOpponent(GridController controllerCopy, MoveGridPart startPos, Pawn opponentPawn, Queue<MoveGridPart> computerPath)
        {

            Pathfinding dijkstra = new Pathfinding();

            MoveGridPart opponentPos = controllerCopy.MoveGridPart.Find(grid => grid.IsWithPawn && grid != startPos);

            opponentPos.IsWithPawn = false;
            Queue<MoveGridPart> opponentPath = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, false);

            MoveGridPart opponentNextPos = opponentPath.Dequeue();
            MoveGridPart opponentNextMove = controllerCopy.MoveGridPart.Find(pos => pos.GridPos.IsEqual(opponentNextPos.GridPos));

            opponentPos.IsWithPawn = true;





            Queue<MoveGridPart> CheckIfOpponentPathExists = null;

            NextStep tryPlaceWall = new NextStep() { isRun = true };

            //if (opponentPos.GridPos.x != 9 && opponentPos.GridPos.y != 9)
            //{



            if (opponentPos.GridPos.y < opponentNextMove.GridPos.y)
            {
                var wall = controllerCopy.WallGridPart.Find(x => x.GridPos.IsEqual(opponentPos.GridPos));
                //Vector2Int i = new Vector2Int() { x = opponentPos.GridPos.x, y = opponentPos.GridPos.y };
                if (opponentPos.GridPos.x != 9 && opponentPos.GridPos.y != 9 && wall.IsHorizontalAllow)
                {


                    controllerCopy.PlaceWall(opponentPos.GridPos, false, false);
                    CheckIfOpponentPathExists = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, true);



                    if (CheckIfOpponentPathExists != null)
                    {
                        tryPlaceWall.isRun = false;
                        tryPlaceWall.isVertical = false;
                        tryPlaceWall.Position = opponentPos.GridPos;
                    }
                }
                else
                    tryPlaceWall.isRun = true;
            }
            else if (opponentPos.GridPos.y > opponentNextMove.GridPos.y)
            {
                var wall = controllerCopy.WallGridPart.Find(x => x.GridPos.IsEqual(opponentNextMove.GridPos));

                //Vector2Int i = new Vector2Int() { x = opponentNextMove.GridPos.x, y = opponentNextMove.GridPos.y };

                if (opponentNextMove.GridPos.x != 9 && opponentNextMove.GridPos.y != 9 && wall.IsHorizontalAllow)
                {
                    controllerCopy.PlaceWall(opponentNextMove.GridPos, false, false);

                    CheckIfOpponentPathExists = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, true);

                    if (CheckIfOpponentPathExists != null)
                    {
                        tryPlaceWall.isRun = false;
                        tryPlaceWall.isVertical = false;
                        tryPlaceWall.Position = opponentNextMove.GridPos;
                    }
                }
                else
                    tryPlaceWall.isRun = true;
                
            }
            else if (opponentPos.GridPos.x < opponentNextMove.GridPos.x)
            {
                if (opponentPos.GridPos.x != 9 && opponentPos.GridPos.y != 9)
                {

                    Vector2Int i = new Vector2Int() { x = opponentPos.GridPos.x, y = opponentPos.GridPos.y };
                    controllerCopy.PlaceWall(i, true, false);
                    CheckIfOpponentPathExists = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, true);

                    if (CheckIfOpponentPathExists != null)
                    {
                        tryPlaceWall.isRun = true;
                        tryPlaceWall.isVertical = true;
                        tryPlaceWall.Position = opponentNextMove.GridPos;
                    }
                }
                else
                    tryPlaceWall.isRun = true;

            }
            else
            {
                if (opponentNextMove.GridPos.x != 9 && opponentNextMove.GridPos.y != 9)
                {

                    Vector2Int i = new Vector2Int() { x = opponentNextMove.GridPos.x, y = opponentNextMove.GridPos.y };
                    controllerCopy.PlaceWall(i, true, false);
                    CheckIfOpponentPathExists = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, true);

                    if (CheckIfOpponentPathExists != null)
                    {
                        tryPlaceWall.isRun = true;
                        tryPlaceWall.isVertical = true;
                        tryPlaceWall.Position = opponentNextMove.GridPos;
                    }

                }
                else
                    tryPlaceWall.isRun = true;

            }

            //}
            //else
            //tryPlaceWall.isRun = true;

            if (tryPlaceWall.isRun != true)
            {
                return tryPlaceWall;
            }
            else
                return new NextStep() { isRun = true, Move = computerPath.Dequeue() };

            #region
            /*
             //find what position of wall will block opponentNextMove

            if(controllerCopy.WallGridPart.Find(w => w.GridPos.IsEqual(tryPlaceWall.Position)).IsHorizontalAllow == false ||
                controllerCopy.WallGridPart.Find(w => w.GridPos.IsEqual(tryPlaceWall.Position)).IsVerticalAllow == false)
            {
                return new NextStep() { isRun = true, Move = computerPath.Dequeue() };
            }

            controllerCopy.PlaceWall(tryPlaceWall.Position, tryPlaceWall.isVertical, false);

            Queue<MoveGridPart> checkOpponentPath = RunMove(opponentPos, controllerCopy.MoveGridPart, dijkstra, opponentPawn, true);

            if(checkOpponentPath != null)
            {
                if(checkOpponentPath.Count > 0)
                {
                    return new NextStep() { isRun = false, Position = tryPlaceWall.Position, isVertical = tryPlaceWall.isVertical };
                }

            }

            return new NextStep() { isRun = true, Move = computerPath.Dequeue() };
            */
            #endregion

        }




        public NextStep NextMove(GridController controllerCopy, MoveGridPart startPos, List<MoveGridPart> field, Pawn pawn, Pawn opponentPawn, bool isPlayerPawn = false)
        {
            Pathfinding dijkstra = new Pathfinding();

            MoveGridPart opponentPos = field.Find(grid => grid.IsWithPawn && grid != startPos);
            MoveGridPart testGrid = field.Find(grid => grid.GridPos.x == opponentPos.GridPos.x && grid.GridPos.y == opponentPos.GridPos.y);
            Queue<MoveGridPart> computerPath = RunMove(startPos, field, dijkstra, pawn, false);
            Queue<MoveGridPart> playerPath = RunMove(opponentPos, field, dijkstra, opponentPawn, true);

            if (playerPath.Count < computerPath.Count)
            {
                //Console.WriteLine("Opponent is winning " + playerPath.Count, "position: " + opponentPos.GridPos.x + ", " + opponentPos.GridPos.y);
                //return computerPath.Dequeue();

                return TryBlockOpponent(controllerCopy, startPos, opponentPawn, computerPath);
            }
            else
                return new NextStep() { isRun = true, Move = computerPath.Dequeue() }; // computerPath.Dequeue();

        }

        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, Pawn pawn, bool isPlayerPawn)
        {
            int finalY = pawn.StartY;

            if (finalY == 9)
                finalY = 1;
            else if (finalY == 1)
                finalY = 9;

            if (isPlayerPawn)
            {
                startPos.IsWithPawn = false;
            }

            List<MoveGridPart> finalPositions = new List<MoveGridPart>();

            for (int x = 1; x <= 9; x++)
            {
                finalPositions.Add(field.Find(gridPart => gridPart.GridPos.y == finalY && gridPart.GridPos.x == x));
            }

            int minPathCount = Int32.MaxValue;
            Queue<MoveGridPart> path = new Queue<MoveGridPart>();


            foreach (MoveGridPart finalPosition in finalPositions)
            {
                Queue<MoveGridPart> pathToCheck = dijkstra.Dijkstra3(finalPosition, startPos, field);

                if (pathToCheck == null)
                    continue;

                if (pathToCheck.Count < minPathCount)
                {
                    minPathCount = pathToCheck.Count;
                    path = pathToCheck;
                }

            }

            if (isPlayerPawn)
            {
                startPos.IsWithPawn = true;
            }

            return path;
        }


        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, bool isPlayerPawn, int y)
        {
            int finalY = y;

            startPos.IsWithPawn = false;

            List<MoveGridPart> finalPositions = new List<MoveGridPart>();

            for (int x = 1; x <= 9; x++)
            {
                finalPositions.Add(field.Find(gridPart => gridPart.GridPos.y == finalY && gridPart.GridPos.x == x));
            }

            int minPathCount = Int32.MaxValue;
            Queue<MoveGridPart> path = new Queue<MoveGridPart>();


            foreach (MoveGridPart finalPosition in finalPositions)
            {
                Queue<MoveGridPart> pathToCheck = dijkstra.Dijkstra3(finalPosition, startPos, field);

                if (pathToCheck == null)
                    continue;

                if (pathToCheck.Count < minPathCount)
                {
                    minPathCount = pathToCheck.Count;
                    path = pathToCheck;
                }

            }

            startPos.IsWithPawn = true;

            return path;
        }
    }
}
