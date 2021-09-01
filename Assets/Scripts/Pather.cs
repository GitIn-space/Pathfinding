using System.Collections.Generic;
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
        [HideInInspector] private List<Customtile> tilevisited = new List<Customtile>();

        [HideInInspector] public struct Customtile
        {
            public TileBase tile;
            public int cost;
            public int distance;
            public Vector2Int parent;
            public Vector2Int pos;
            public bool visited;
        }

        private void Setdistance(Customtile subject)
        {
            subject.distance = Mathf.Abs(goal.pos.x - subject.pos.x) + Mathf.Abs(goal.pos.y - subject.pos.y);
        }

        private void Addneighbour(Customtile subject)
        {
            ;
            //cost: parent+1
            //distance: neighbour -> goal
        }

        public List<Customtile> Astar()
        {
            List<Customtile> path = new List<Customtile>();

            Customtile active;

            while(tilequeue.Count > 0)
            {
                active = tilequeue[0];
                for (int c = 1; c < tilequeue.Count; c++)
                    if (tilequeue[c].distance < active.distance)
                    {
                        active = tilequeue[c];
                        tilevisited.Add(active);
                        tilequeue.RemoveAt(c);
                    }

                if (active.pos.x == goal.pos.x && active.pos.y == goal.pos.y)
                    ;//return

                if (active.pos.x + 1 < 14 && tilewalkable[active.pos.y, active.pos.x + 1].tile != null)
                {
                    if (tilewalkable[active.pos.y, active.pos.x + 1].visited)
                        continue;
                    for(int c = 0; c < tilequeue.Count; c++)
                    {
                        
                    }
                }

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
                        start.tile = tiles[i];
                        start.pos.x = x;
                        start.pos.y = y;
                    }
                    if (tiles[i].name == "Goal")
                    {
                        goal.tile = tiles[i];
                        goal.pos.x = x;
                        goal.pos.y = y;
                    }

                    tilewalkable[y, x].tile = tiles[i];
                    tilewalkable[y, x].pos.x = x;
                    tilewalkable[y, x].pos.y = y;
                    Setdistance(tilewalkable[y, x]);
                }
            Setdistance(start);
            tilequeue.Add(start);
        }
    }
}