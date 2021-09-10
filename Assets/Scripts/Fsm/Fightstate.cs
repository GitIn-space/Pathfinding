namespace FG
{
    public class Fightstate : State
    {
        public Fightstate() : base()
        {
        }

        public Fightstate(int health, int ammo) : base(health, ammo)
        {
            //stand still
        }

        public override State Execute(ref int trigger)
        {
            //shoot nearest enemy within los
            //if hp/ammo low -> lootstate
            //if no enemy within los -> huntstate
            return this;
        }
    }
}