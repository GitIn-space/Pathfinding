using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Agent : MonoBehaviour
    {
        [HideInInspector] private List<Vector3> path;
        [HideInInspector] private Coroutine updateroutine;
        [HideInInspector] private State state;
        [HideInInspector] private Agenthandler handler;
        [HideInInspector] private int id;
        [HideInInspector] private int pathtrigger = 1;
        [HideInInspector] private Vector3 target;

        [SerializeField] private float movementspeed = 1.0f;
        [SerializeField] private float initialwait = 1.0f;
        [SerializeField] private GameObject bulletprefab;
        [SerializeField] private int bulletforce = 1;

        private IEnumerator Agentupdate()
        {
            yield return new WaitForSeconds(initialwait);

            for (int c = 0; c < path.Count; c++)
            {
                state = state.Execute(ref pathtrigger);

                if (state.GetType() == typeof(Fightstate))
                {
                    if (state.Shoot())
                    {
                        Bullet bullet = Instantiate(bulletprefab, transform.position, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Bullet>();
                        bullet.GetComponent<Rigidbody2D>().AddForce((target - transform.position).normalized * bulletforce, ForceMode2D.Force);
                    }
                }

                if (pathtrigger != 0)
                {
                    path = handler.Requestpath(id, pathtrigger);
                    pathtrigger = 0;
                }

                if (state.GetType() != typeof(Fightstate))
                    transform.position = path[c];

                if (c == path.Count - 1)
                {
                    path = handler.Requestpath(id);
                    c = 0;
                }

                yield return new WaitForSeconds(movementspeed);
            }
        }

        public void Receivevisual(Vector3 target)
        {
            if (Vector3.Distance(transform.position, target) < Vector3.Distance(transform.position, this.target))
            {
                this.target = target;
            }

            state = state.Visualcontact();
        }

        public void Takedamage(int damage)
        {
            state.Takedamage(damage);
        }

        public void Setid(int id)
        {
            this.id = id;
        }

        private void OnDisable()
        {
            StopCoroutine(updateroutine);
        }

        private void Awake()
        {
            path = new List<Vector3>();
            state = new Huntstate();
            target = GameObject.Find("Distantobject").transform.position;
        }

        private void Start()
        {
            handler = GameObject.Find("Agenthandler").GetComponent<Agenthandler>();
            path = handler.Requestpath(id);
            updateroutine = StartCoroutine("Agentupdate");
        }
    }
}