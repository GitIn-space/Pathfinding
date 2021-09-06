using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Agent : MonoBehaviour
    {
        [HideInInspector] private List<Customtile> path;
        [HideInInspector] private Coroutine pathrutine;

        //[SerializeField] private float timebeforefirstmove = 1.0f;
        [SerializeField] private float movementspeed = 1.0f;

        private IEnumerator Move()
        {
           for(int c = path.Count - 1; c > -1; c--)
            {
                transform.position = new Vector3(path[c].pos.x, path[c].pos.y, 0);
                yield return new WaitForSeconds(movementspeed);
            }
            Destroy(gameObject);
        }

        public void Givepath(List<Customtile> path)
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