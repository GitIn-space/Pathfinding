using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        private void Destroybullet()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Agent") || !collision.collider.CompareTag("Wall"))
                return;

            if (collision.collider.CompareTag("Agent"))
            {
                Agent enemy = collision.collider.GetComponent<Agent>();
                if (!ReferenceEquals(enemy, null))
                {
                    enemy.Takedamage(damage);
                    Destroybullet();
                }
                else
                    Destroybullet();
            }
            else
                Destroybullet();
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}