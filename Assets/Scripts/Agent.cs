using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Agent : MonoBehaviour
    {
        [HideInInspector] private List<Vector3> path;
        [HideInInspector] private Coroutine pathrutine;

        [SerializeField] private float movementspeed = 1.0f;

        private IEnumerator Move()
        {
           for(int c = path.Count - 1; c > -1; c--)
            {
                transform.position = path[c];
                yield return new WaitForSeconds(movementspeed);
            }
            Destroy(gameObject);
        }

        public void Givepath(List<Vector3> path)
        {
            this.path = path;

            pathrutine = StartCoroutine(Move());
        }

        private void OnDisable()
        {
            StopCoroutine(pathrutine);
        }
    }
}