using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorConsole
{
    public class Pathfinding
    {
        public void Dijkstra(List<MoveGridPart> gridParts, MoveGridPart currentPos)
        {

            List<MoveGridPart> visited = new List<MoveGridPart>();
            List<MoveGridPart> unvisited = new List<MoveGridPart>(gridParts.Where(x => x != currentPos));

            MoveGridPart current = currentPos;

            List<MoveGridPart> finalVerticles = new List<MoveGridPart>(gridParts.Where(x => x.GridPos.y == 1));

            Queue<MoveGridPart> Queue = new Queue<MoveGridPart>();

            foreach (MoveGridPart g in unvisited)
            {
                foreach (MoveGridPart neighbour in GetNeighbours(current, gridParts))
                {


                }
            }
        }


        private List<MoveGridPart> CalculatePath(MoveGridPart endNode)
        {
            return null;
        }


        public Queue<MoveGridPart> Dijkstra3(MoveGridPart startPos, MoveGridPart goalPos, List<MoveGridPart> gridPArts)
        {
            Dictionary<MoveGridPart, MoveGridPart> nextPosToGoal = new Dictionary<MoveGridPart, MoveGridPart>();
            Dictionary<MoveGridPart, int> costToReachPos = new Dictionary<MoveGridPart, int>();

            PriorityQueue<MoveGridPart> frontier = new PriorityQueue<MoveGridPart>();

            frontier.Enqueue(startPos, 0);
            costToReachPos[startPos] = 0;

            while (frontier.Count > 0)
            {
                MoveGridPart curPos = frontier.Dequeue();

                if (curPos == goalPos)
                    break;

                List<MoveGridPart> neigbours = GetNeighbours(curPos, gridPArts);
                foreach (MoveGridPart neighbour in neigbours)
                {
                    int newCost = costToReachPos[curPos] + neighbour.Cost;

                    if (costToReachPos.ContainsKey(neighbour) == false || newCost < costToReachPos[neighbour])
                    {
                        costToReachPos[neighbour] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(neighbour, priority);
                        nextPosToGoal[neighbour] = curPos;
                    }
                }
            }
            if (nextPosToGoal.ContainsKey(goalPos) == false)
                return null;

            Queue<MoveGridPart> path = new Queue<MoveGridPart>();
            MoveGridPart curPathTile = goalPos;
            while (curPathTile != startPos)
            {
                curPathTile = nextPosToGoal[curPathTile];
                path.Enqueue(curPathTile);
            }

            return path;
        }


        public List<MoveGridPart> GetNeighbours(MoveGridPart current, List<MoveGridPart> gridPArts)
        {
            List<MoveGridPart> neighbours = new List<MoveGridPart>();
            var move = PossibleMove.GetPossibleMoves(gridPArts, current);


            foreach (Vector2Int v in move)
            {
                MoveGridPart g = gridPArts.Find(x => x.GridPos.x == v.x && x.GridPos.y == v.y);
                if (g != null)
                    neighbours.Add(g);
            }


            return neighbours;
        }
    }
}
