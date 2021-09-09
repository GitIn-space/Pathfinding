namespace FG
{
    public class Fightstate : State
    {
        public Fightstate() : base()
        {
            //get path to nearest enemy
        }

        public override State Execute()
        {
            //shoot nearest enemy within los
            //if hp/ammo low -> lootstate
            //if no enemy within los -> huntstate
        }
    }
}