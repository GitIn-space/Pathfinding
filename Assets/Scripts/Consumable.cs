using UnityEngine;

namespace FG
{
    public class Consumable : MonoBehaviour
    {
        [HideInInspector] private Coroutine activateroutine;
        [HideInInspector] private Pather pathfinder;

        [SerializeField] private int healing = 50;
        [SerializeField] private int ammo = 5;

        private void Consume()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Agent"))
            {
                Agent agent = collision.GetComponent<Agent>();
                int type = gameObject.name == "Health" ? 0 : 1;
                int amount = type == 0 ? healing : ammo;
                agent.Consume(type, amount);
                Consume();
            }
        }

        private void Awake()
        {
            pathfinder = GetComponentInParent<Pather>();
        }
    }
}