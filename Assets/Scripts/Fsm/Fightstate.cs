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
            if (health <= 20)
            {
                trigger = 1;
                return new Lootstate(health, ammo);
            }
            else if (ammo == 0)
            {
                trigger = 1;
                return new Lootstate(health, ammo);
            }
            else if(false)
            {
                //shoot nearest enemy within los
                return this;
            }
            else
                return new Huntstate(health, ammo);
        }
    }
}