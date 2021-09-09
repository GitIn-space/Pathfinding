using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Agent : MonoBehaviour
    {
        [HideInInspector] private List<Vector3> path;
        [HideInInspector] private Coroutine pathrutine;
        [HideInInspector] private State state;
        [HideInInspector] private Agenthandler handler;
        [HideInInspector] private int id;

        [SerializeField] private float movementspeed = 1.0f;

        private IEnumerator Move()
        {
           for(int c = path.Count - 1; c > -1; c--)
            {
                transform.position = path[c];
                yield return new WaitForSeconds(movementspeed);
            }
        }

        public void Setid(int id)
        {
            this.id = id;
        }

        private void OnDisable()
        {
            StopCoroutine(pathrutine);
        }

        private void Awake()
        {
            state = new Huntstate();
            handler = GetComponentInParent<Agenthandler>();
        }
    }
}