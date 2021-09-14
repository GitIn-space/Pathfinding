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
        [HideInInspector] private int pathtrigger;

        [SerializeField] private float movementspeed = 1.0f;
        [SerializeField] private float initialwait = 1.0f;

        private IEnumerator Agentupdate()
        {
            yield return new WaitForSeconds(initialwait);

            state = state.Execute(ref pathtrigger);
            if (pathtrigger != 0)
            {
                path = handler.Requestpath(id, pathtrigger);
                pathtrigger = 0;
            }

            for (int c = 0; c < path.Count; c++)
            {
                transform.position = path[c];

                if (state.GetType() == typeof(Huntstate))
                    c--;

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
        }

        private void Start()
        {
            handler = GameObject.Find("Agenthandler").GetComponent<Agenthandler>();
            path = handler.Requestpath(id);
            updateroutine = StartCoroutine("Agentupdate");
        }
    }
}