namespace FG
{
    public class Huntstate : State
    {
        public Huntstate() : base()
        {
        }

        public Huntstate(int health, int ammo) : base(health, ammo)
        {
            //get path to nearest enemy
        }

        public override State Execute(ref int trigger)
        {
            //if enemy in los -> fightstate
            //if hp/ammo low -> lootstate
            return this;
        }
    }
}