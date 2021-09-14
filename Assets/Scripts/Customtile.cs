using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG
{
    public class Customtile
    {
        public Vector2Int pos;
        public float costdistance;
        public int cost;
        public float distance;
        public Customtile Parent;
        public TileBase tile;

        public Customtile()
        {
            pos = new Vector2Int(-1, -1);
        }
        public Customtile(int x, int y, TileBase tile = null)
        {
            pos.x = x;
            pos.y = y;
            this.tile = tile;
        }
    }
}