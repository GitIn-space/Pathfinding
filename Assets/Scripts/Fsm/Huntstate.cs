namespace FG
{
    public class Huntstate : State
    {
        public Huntstate() : base()
        {
            //get path to nearest enemy
        }

        public override State Execute()
        {
            //path to enemy
            //if enemy in los -> fightstate
            //if hp/ammo low -> lootstate
        }
    }
}