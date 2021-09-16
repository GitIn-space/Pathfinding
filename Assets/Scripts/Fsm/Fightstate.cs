using UnityEngine;

namespace FG
{
    public class Fightstate : State
    {
        public Fightstate() : base()
        {
        }

        public Fightstate(int health, int ammo) : base(health, ammo)
        {
        }

        public override State Execute(ref int trigger)
        {
            if (health <= 50)
            {
                trigger = 2;
                return new Lootstate(health, ammo);
            }
            else if (ammo == 0)
            {
                trigger = 3;
                return new Lootstate(health, ammo);
            }
            else
            {
                return this;
            }
        }

        public override bool Shoot()
        {
            if (ammo > 0)
                ;// ammo--;
            else
                return false;
            return true;
        }
    }
}