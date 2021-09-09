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
        abstract public State Execute();
        public void Exit(State state)
        {
        }
    }
}