namespace FG
{
    public class Huntstate : State
    {
        public Huntstate() : base()
        {
        }

        public Huntstate(int health, int ammo) : base(health, ammo)
        {
        }

        public override State Execute(ref int trigger)
        {
            //if hp/ammo low -> lootstate
            return this;
        }
    }
}