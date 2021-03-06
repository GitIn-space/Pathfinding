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
        [HideInInspector] private int pathtrigger = 0;
        [HideInInspector] private Vector3 target;
        [HideInInspector] private bool retarget = false;

        [SerializeField] private float movementspeed = 1.0f;
        [SerializeField] private GameObject bulletprefab;
        [SerializeField] private int bulletforce = 1;

        private IEnumerator Agentupdate()
        {
            for (int c = 0; c < path.Count;)
            {
                state = state.Execute(ref pathtrigger);

                if (state.GetType() == typeof(Fightstate))
                    if (state.Shoot())
                    {
                        Bullet bullet = Instantiate(bulletprefab, transform.position, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Bullet>();
                        bullet.GetComponent<Rigidbody2D>().AddForce((target - transform.position).normalized * bulletforce, ForceMode2D.Force);
                    }

                if (pathtrigger != 0)
                {
                    path = handler.Requestpath(id, pathtrigger);
                    pathtrigger = 0;
                    retarget = true;
                }

                if (c >= path.Count - 1)
                {
                    path = handler.Requestpath(id, 1);
                    retarget = true;
                }

                yield return new WaitForSeconds(movementspeed);

                if (retarget)
                {
                    c = 1;
                    retarget = false;
                }

                if (state.GetType() != typeof(Fightstate))
                {
                    transform.position = path[c];
                    c++;
                }
            }
        }

        public void Receivevisual(Vector3 target)
        {
            if (target != transform.position)
                this.target = target;

            if(state.GetType() != typeof(Lootstate))
                state = state.Visualcontact();
        }

        public void Targetdown(Vector3 target)
        {
            //if (this.target == target)
            {
                state = state.Targetdown();
                path = handler.Requestpath(id, 1);
                retarget = true;
                this.target = handler.Getfartarget();
            }
        }

        public void Takedamage(int damage)
        {
            if (state.Takedamage(damage) <= 0)
            {
                handler.Dead(id);
                StopCoroutine(updateroutine);
                gameObject.SetActive(false);
            }
        }

        public void Consume(int type, int amount)
        {
            state.Consume(type, amount);
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
        }

        private void Start()
        {
            handler = GameObject.Find("Agenthandler").GetComponent<Agenthandler>();
            path = handler.Requestpath(id, 1);
            target = handler.Getfartarget();
            updateroutine = StartCoroutine("Agentupdate");
        }
    }
}