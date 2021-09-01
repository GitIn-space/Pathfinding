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
            public TileBase parent;
            public int x;
            public int y;
            public bool visited;
        }

        private void Setdistance(Customtile subject)
        {
            subject.cost = Mathf.Abs(goal.x - subject.x) + Mathf.Abs(goal.y - subject.y);
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

                if (active.x == goal.x && active.y == goal.y)
                    ;//return


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
                        start.x = x;
                        start.y = y;
                    }
                    if (tiles[i].name == "Goal")
                    {
                        goal.tile = tiles[i];
                        goal.x = x;
                        goal.y = y;
                    }

                    tilewalkable[y, x].tile = tiles[i];
                    tilewalkable[y, x].x = x;
                    tilewalkable[y, x].y = y;
                    tilewalkable[y, x].distance = Mathf.Abs(goal.x - tilewalkable[y, x].x + goal.y - tilewalkable[y, x].y);
                }
            Setdistance(start);
            tilequeue.Add(start);
        }
    }
}