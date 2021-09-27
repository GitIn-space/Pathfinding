using UnityEngine;

namespace FG
{
    public class Lootstate : State
    {
        public Lootstate() : base()
        {
        }

        public Lootstate(int health, int ammo) : base(health, ammo)
        {
        }

        public override State Execute(ref int trigger)
        {
            if (health >= 50 && ammo > 0)
            {
                trigger = 1;
                return new Huntstate(health, ammo);
            }
            return this;
        }
    }
}