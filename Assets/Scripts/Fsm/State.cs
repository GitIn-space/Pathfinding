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
            this.health = 100;
            this.ammo = 10;
        }

        abstract public State Execute(ref int trigger);

        public void Takedamage(int damage)
        {
            health -= damage;
        }

        public State Visualcontact()
        {
            return new Fightstate(health, ammo);
        }
    }
}