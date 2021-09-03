using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Agent : MonoBehaviour
    {
        [HideInInspector] private List<Pather.Customtile> path;
        [HideInInspector] private Coroutine pathrutine;

        [SerializeField] private float timebeforefirstmove = 1.0f;
        [SerializeField] private float movementspeed = 1.0f;

        private IEnumerator Move()
        {
            yield return new WaitForSeconds(timebeforefirstmove);

            int c = path.Count - 1;
            while (true)
            {
                transform.position = new Vector3(path[c].pos.x, path[c].pos.y, 0);
                c--;
                yield return new WaitForSeconds(movementspeed);

                if (c == 0)
                    break;
            }
        }

        public void Givepath(List<Pather.Customtile> path)
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