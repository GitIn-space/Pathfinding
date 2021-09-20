using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Bullet : MonoBehaviour
    {
        [HideInInspector] private Coroutine activateroutine;

        [SerializeField] private int damage = 10;
        [SerializeField] private float activatedelay = 0.1f;

        private void Destroybullet()
        {
            Destroy(gameObject);
        }

        private IEnumerator Collideractivate()
        {
            yield return new WaitForSeconds(activatedelay);

            GetComponent<CircleCollider2D>().enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Agent"))
            {
                Agent enemy = collision.GetComponent<Agent>();
                if (!ReferenceEquals(enemy, null))
                {
                    enemy.Takedamage(damage);
                }
                else
                    Destroybullet();
            }
            else if(collision.CompareTag("Wall"))
                Destroybullet();
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            StopCoroutine(activateroutine);
        }

        private void Awake()
        {
            activateroutine = StartCoroutine("Collideractivate");
        }
    }
}