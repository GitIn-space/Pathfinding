using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG
{
    public class Customtile
    {
        public Vector2Int pos;
        public int costdistance;
        public int cost;
        public int distance;
        public Customtile Parent;
        public TileBase tile;

        public Customtile()
        {
            pos = new Vector2Int(-1, -1);
        }
        public Customtile(int x, int y)
        {
            pos.x = x;
            pos.y = y;
        }
    }

    public class Pather : MonoBehaviour
    {
        [HideInInspector] private Tilemap tilemap;
        [HideInInspector] private Customtile[,] grid = new Customtile[14, 10];
        [HideInInspector] private Customtile start;
        [HideInInspector] private Customtile goal;
        [HideInInspector] private BoundsInt bounds;
        [HideInInspector] private float offx;
        [HideInInspector] private float offy;

        private List<Customtile> Getneighbours(int x, int y)
        {
            List<Customtile> directions = new List<Customtile>();
            if (y - 1 >= 0)
                directions.Add(new Customtile(x, y - 1));
            if (y + 1 < 10)
                directions.Add(new Customtile(x, y + 1));
            if (x - 1 >= 0)
                directions.Add(new Customtile(x - 1, y));
            if (x + 1 < 14)
                directions.Add(new Customtile(x + 1, y));

            return directions.Where(l => grid[l.pos.x, l.pos.y] != null).ToList();
        }

        static int Setdistance(Customtile subject, Customtile target)
        {
            return Mathf.Abs(target.pos.x - subject.pos.x) + Mathf.Abs(target.pos.y - subject.pos.y);
        }

        public List<Vector3> Astar()
        {
            Customtile current = null;
            List<Customtile> open = new List<Customtile>();
            List<Customtile> closed = new List<Customtile>();
            int g = 0;

            open.Add(start);

            while (open.Count > 0)
            {
                int lowest = open.Min(l => l.costdistance);
                current = open.First(l => l.costdistance == lowest);

                closed.Add(current);

                open.Remove(current);

                if (closed.FirstOrDefault(l => l.pos.x == goal.pos.x && l.pos.y == goal.pos.y) != null)
                    break;

                List<Customtile> neighbours = Getneighbours(current.pos.x, current.pos.y);
                g++;

                foreach (Customtile neighbourtile in neighbours)
                {
                    if (closed.FirstOrDefault(l => l.pos.x == neighbourtile.pos.x
                            && l.pos.y == neighbourtile.pos.y) != null)
                        continue;

                    if (open.FirstOrDefault(l => l.pos.x == neighbourtile.pos.x
                            && l.pos.y == neighbourtile.pos.y) == null)
                    {
                        neighbourtile.cost = g;
                        neighbourtile.distance = Setdistance(neighbourtile, goal);
                        neighbourtile.costdistance = neighbourtile.cost + neighbourtile.distance;
                        neighbourtile.Parent = current;

                        open.Insert(0, neighbourtile);
                    }
                    else
                    {
                        {
                            neighbourtile.cost = g;
                            neighbourtile.costdistance = neighbourtile.cost + neighbourtile.distance;
                            neighbourtile.Parent = current;
                        }
                    }
                }
            }
            List<Vector3> path = new List<Vector3>();
            while(current.pos != start.pos)
            {
                path.Add(tilemap.CellToWorld(new Vector3Int(current.pos.x, current.pos.y, 0)));
                path[path.Count - 1] -= new Vector3(offx, offy, 0);
                current = current.Parent;
            }
            path.Add(tilemap.CellToWorld(new Vector3Int(start.pos.x, start.pos.y, 0)));
            path[path.Count - 1] -= new Vector3(offx, offy, 0);
            return path;
        }

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            bounds = tilemap.cellBounds;
            offx = (Mathf.Abs(bounds.xMin - (bounds.xMax) + 1) / 2) + 0.5f;
            offy = (Mathf.Abs(bounds.yMin - bounds.yMax) / 2) - 0.5f;
            Debug.Log(offx + " " + offy);
            TileBase[] tiles = tilemap.GetTilesBlock(bounds);
            for (int y = 0, i = 0; y < 10; y++)
                for (int x = 0; x < 14; x++, i++)
                {
                    if (tiles[i].name == "Wall")
                    {
                        grid[x, y] = null;
                        continue;
                    }

                    if (tiles[i].name == "Start")
                    {
                        start = new Customtile(x, y);
                        start.tile = tiles[i];
                    }
                    if (tiles[i].name == "Goal")
                    {
                        goal = new Customtile(x, y);
                        goal.tile = tiles[i];
                    }

                    grid[x, y] = new Customtile(x, y);
                    grid[x, y].tile = tiles[i];
                }
            Setdistance(start, goal);
        }
    }
}