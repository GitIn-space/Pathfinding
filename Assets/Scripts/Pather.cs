using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG
{
    public class Pather : MonoBehaviour
    {
        [HideInInspector] private Tilemap tilemap;
        [HideInInspector] private Customtile[,] tilewalkable = new Customtile[10, 14];
        [HideInInspector] private Customtile start;
        [HideInInspector] private Customtile goal;
        [HideInInspector] private List<Customtile> tilequeue = new List<Customtile>();

        public class Customtile
        {
            public Vector2Int pos;
            public int costdistance;
            public int cost;
            public int distance;
            public Customtile Parent;

            public Customtile()
            {
            }
            public Customtile(int x, int y)
            {
                pos.x = x;
                pos.y = y;
            }
        }

        private void Setdistance(Customtile subject, Customtile target)
        {
            subject.distance = Mathf.Abs(subject.pos.x - goal.pos.x) + Mathf.Abs(subject.pos.y - goal.pos.y);
        }

        private List<Customtile> GetWalkableAdjacentSquares(int x, int y)
        {
            List<Customtile> proposedLocations = new List<Customtile>()
            {
                new Customtile(x, y - 1),
                new Customtile(x, y + 1),
                new Customtile(x - 1, y),
                new Customtile(x + 1, y),
            };

            return proposedLocations.Where(l => tilewalkable[l.pos.y, l.pos.x].pos != null).ToList();
        }

        static int ComputeHScore(Customtile subject, Customtile target)
        {
            return Mathf.Abs(target.pos.x - subject.pos.x) + Mathf.Abs(target.pos.y - subject.pos.y);
        }

        public List<Customtile> Astar()
        {
            Customtile current = null;
            var openList = new List<Customtile>();
            var closedList = new List<Customtile>();
            int g = 0;

            // start by adding the original position to the open list
            openList.Add(start);

            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                var lowest = openList.Min(l => l.costdistance);
                current = openList.First(l => l.costdistance == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(l => l.pos.x == goal.pos.x && l.pos.y == goal.pos.y) != null)
                    break;

                List<Customtile> adjacentSquares = GetWalkableAdjacentSquares(current.pos.x, current.pos.y);
                g++;

                foreach (Customtile adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.pos.x == adjacentSquare.pos.x
                            && l.pos.y == adjacentSquare.pos.y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.pos.x == adjacentSquare.pos.x
                            && l.pos.y == adjacentSquare.pos.y) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.cost = g;
                        adjacentSquare.distance = ComputeHScore(adjacentSquare, goal);
                        adjacentSquare.costdistance = adjacentSquare.cost + adjacentSquare.distance;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.distance < adjacentSquare.costdistance)
                        {
                            adjacentSquare.cost = g;
                            adjacentSquare.costdistance = adjacentSquare.cost + adjacentSquare.distance;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }
            List<Customtile> path = new List<Customtile>();
            while(current.pos.x != start.pos.x && current.pos.y != start.pos.y)
            {
                path.Add(current);
                current = current.Parent;
            }
            return path;
        }

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] tiles = tilemap.GetTilesBlock(bounds);
            for (int y = 0, i = 0; y < 10; y++)
                for (int x = 0; x < 14; x++, i++)
                {
                    if (tiles[i].name == "Wall")
                        continue;

                    if (tiles[i].name == "Start")
                    {
                        start = new Customtile(x, y);
                    }
                    if (tiles[i].name == "Goal")
                    {
                        goal = new Customtile(x, y);
                    }

                    tilewalkable[y, x] = new Customtile(x, y);
                }
            Setdistance(start, goal);
            tilequeue.Add(start);
        }
    }
}