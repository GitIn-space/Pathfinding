namespace FG
{
    public class Lootstate : State
    {
        public Lootstate() : base()
        {
        }

        public Lootstate(int health, int ammo) : base(health, ammo)
        {
            //get path to nearest health/ammo
        }

        public override State Execute(ref int trigger)
        {
            //if health/ammo satisfied -> huntstate
            return this;
        }
    }
}