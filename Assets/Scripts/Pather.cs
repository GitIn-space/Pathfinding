using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG
{
    public class Pather : MonoBehaviour
    {
        [HideInInspector] private Tilemap tilemap;
        [HideInInspector] private Customtile[,] grid;
        [HideInInspector] private List<Customtile> starts;
        [HideInInspector] private Customtile goal;
        [HideInInspector] private BoundsInt bounds;
        [HideInInspector] private int offx;
        [HideInInspector] private int offy;
        [HideInInspector] private int gridlengthx;
        [HideInInspector] private int gridlengthy;

        private List<Customtile> Getneighbours(int x, int y)
        {
            List<Customtile> directions = new List<Customtile>();
            if (y - 1 >= 0)
                directions.Add(new Customtile(x, y - 1));
            if (y + 1 < gridlengthy)
                directions.Add(new Customtile(x, y + 1));
            if (x - 1 >= 0)
                directions.Add(new Customtile(x - 1, y));
            if (x + 1 < gridlengthx)
                directions.Add(new Customtile(x + 1, y));

            return directions.Where(l => grid[l.pos.x, l.pos.y] != null).ToList();
        }

        private float Getdistance(Vector2Int subject, Vector2Int target)
        {
            return Vector2Int.Distance(subject, target);
        }

        private Vector3 Addoffset(Vector3 pos)
        {
            return pos - new Vector3Int(offx, offy, 0);
        }

        private Vector3Int Removeoffset(Vector3Int pos)
        {
            return pos + new Vector3Int(offx, offy, 0);
        }

        private void Addtolist(ref List<Vector3> list, Vector3Int vector)
        {
            list.Add(tilemap.CellToWorld(new Vector3Int(vector.x, vector.y, 0)));
            list[list.Count - 1] = Addoffset(list[list.Count - 1]);
        }

        public List<Vector3> Astar(Vector3 location, Vector3 target)
        {
            Customtile start = new Customtile();
            Customtile goal = new Customtile();

            Vector3Int worldcell = tilemap.WorldToCell(location);
            worldcell = Removeoffset(new Vector3Int(worldcell.x, worldcell.y, 0));
            start.pos.x = worldcell.x;
            start.pos.y = worldcell.y;

            worldcell = tilemap.WorldToCell(target);
            worldcell = Removeoffset(new Vector3Int(worldcell.x, worldcell.y, 0));
            goal.pos.x = worldcell.x;
            goal.pos.y = worldcell.y;

            Customtile current = null;
            List<Customtile> open = new List<Customtile>();
            List<Customtile> closed = new List<Customtile>();
            int cost = 0;

            open.Add(start);

            while (open.Count > 0)
            {
                float lowest = open.Min(l => l.costdistance);
                current = open.First(l => l.costdistance == lowest);

                closed.Add(current);

                open.Remove(current);

                if (closed.FirstOrDefault(l => l.pos.x == goal.pos.x && l.pos.y == goal.pos.y) != null)
                    break;

                List<Customtile> neighbours = Getneighbours(current.pos.x, current.pos.y);
                cost++;

                foreach (Customtile neighbourtile in neighbours)
                {
                    if (closed.FirstOrDefault(l => l.pos.x == neighbourtile.pos.x && l.pos.y == neighbourtile.pos.y) != null)
                        continue;

                    if (open.FirstOrDefault(l => l.pos.x == neighbourtile.pos.x
                            && l.pos.y == neighbourtile.pos.y) == null)
                    {
                        neighbourtile.cost = cost;
                        neighbourtile.distance = Getdistance(neighbourtile.pos, goal.pos);
                        neighbourtile.costdistance = neighbourtile.cost + neighbourtile.distance;
                        neighbourtile.Parent = current;

                        open.Insert(0, neighbourtile);
                    }
                    else
                    {
                        if (cost + neighbourtile.distance < neighbourtile.costdistance)
                        {
                            neighbourtile.cost = cost;
                            neighbourtile.costdistance = neighbourtile.cost + neighbourtile.distance;
                            neighbourtile.Parent = current;
                        }
                    }
                }
            }
            List<Vector3> path = new List<Vector3>();
            while(current.pos != start.pos)
            {
                Addtolist(ref path, new Vector3Int(current.pos.x, current.pos.y, 0));
                current = current.Parent;
            }
            Addtolist(ref path, new Vector3Int(start.pos.x, start.pos.y, 0));
            path.Reverse();
            return path;
        }

        public List<Vector3> Getstartlocations()
        {
            List<Vector3> startreturn = new List<Vector3>();

            for(int c = 0; c < starts.Count; c++)
                Addtolist(ref startreturn, new Vector3Int(starts[c].pos.x, starts[c].pos.y, 0));
            return startreturn;
        }

        private void Awake()
        {
            starts = new List<Customtile>();
            tilemap = GetComponent<Tilemap>();
            tilemap.CompressBounds();
            bounds = tilemap.cellBounds;
            gridlengthx = Mathf.Abs(bounds.xMin - bounds.xMax);
            gridlengthy = Mathf.Abs(bounds.yMin - bounds.yMax);
            grid = new Customtile[gridlengthx, gridlengthy];
            offx = (Mathf.Abs((bounds.xMin - bounds.xMax) + 1) / 2) + 1;
            offy = (Mathf.Abs((bounds.yMin - bounds.yMax) + 1) / 2) - 1;
            TileBase[] tiles = tilemap.GetTilesBlock(bounds);
            for (int y = 0, i = 0; y < gridlengthy; y++)
                for (int x = 0; x < gridlengthx; x++, i++)
                {
                    if (tiles[i].name == "Wall")
                    {
                        grid[x, y] = null;
                        continue;
                    }
                    if (tiles[i].name == "Start")
                    {
                        starts.Add(new Customtile(x, y, tiles[i]));
                    }
                    if (tiles[i].name == "Goal")
                    {
                        goal = new Customtile(x, y, tiles[i]);
                    }
                    grid[x, y] = new Customtile(x, y, tiles[i]);
                }
        }
    }
}