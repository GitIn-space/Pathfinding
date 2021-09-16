namespace FG
{
    abstract public class State
    {
        protected int health;
        protected int ammo;

        public State()
        {
            health = 100;
            ammo = 10;
        }

        public State(int health, int ammo)
        {
            this.health = health;
            this.ammo = ammo;
        }

        abstract public State Execute(ref int trigger);

        public virtual int Takedamage(int damage)
        {
            health -= damage;
            return health;
        }

        public virtual State Visualcontact()
        {
            return new Fightstate(health, ammo);
        }

        public virtual State Targetdown()
        {
            return new Huntstate(health, ammo);
        }

        public virtual bool Shoot()
        {
            return false;
        }
    }
}