using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG
{
    public class Agenthandler : MonoBehaviour
    {
        [HideInInspector] private List<Agent> agents = new List<Agent>();
        [HideInInspector] private Pather pathfinder;
        [HideInInspector] private Coroutine visionroutine;

        [SerializeField] private GameObject agentprefab;
        [SerializeField] private float visionrange = 5.0f;
        [SerializeField] private float visionupdaterate = 1.0f;

        public List<Vector3> Requestpath(int id, int pathtrigger = 0)
        {
            Vector3 target = GameObject.Find("Distantobject").transform.position;

            if (pathtrigger == 0)
            {
                for (int c = 0; c < agents.Count; c++)
                {
                    if (c != id)
                        if (Vector3.Distance(agents[id].transform.position, agents[c].transform.position) <
                            Vector3.Distance(target, agents[c].transform.position))
                            target = agents[c].transform.position;
                }
            }
            else if (pathtrigger == 1)
            {
                //get path to ammo/health
            }
            return pathfinder.Astar(agents[id].transform.position, target);
        }

        public IEnumerator Checkvision()
        {
            while (true)
            {
                List<Vector2Int> sightpotential = new List<Vector2Int>();
                for (int c = 0; c < agents.Count - 1; c++)
                    for (int q = c + 1; q < agents.Count; q++)
                        if (Vector3.Distance(agents[c].transform.position, agents[q].transform.position) < visionrange)
                        {
                            sightpotential.Add(new Vector2Int(c, q));
                            Debug.DrawRay(agents[c].transform.position, (agents[q].transform.position - agents[c].transform.position).normalized, Color.red, 1f);
                        }

                RaycastHit2D hit;
                Vector3 direction;

                for (int c = 0; c < sightpotential.Count; c++)
                {
                    agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = false;

                    direction = (agents[sightpotential[c].y].transform.position - agents[sightpotential[c].x].transform.position).normalized;
                    hit = Physics2D.Raycast(agents[sightpotential[c].x].transform.position, direction, visionrange);

                    agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = true;

                    if (hit.collider)
                    {
                        if ((hit.collider.CompareTag("Agent")))
                        {
                            agents[sightpotential[c].x].Receivevisual(agents[sightpotential[c].y].transform.position);
                            agents[sightpotential[c].y].Receivevisual(agents[sightpotential[c].x].transform.position);
                            Debug.DrawRay(agents[sightpotential[c].x].transform.position, (agents[sightpotential[c].y].transform.position - agents[sightpotential[c].x].transform.position).normalized, Color.cyan, 60f);
                        }
                    }
                }
                yield return new WaitForSeconds(visionupdaterate);
            }
        }

        private void Awake()
        {
            pathfinder = GetComponentInParent<Pather>();
        }

        private void Start()
        {
            List<Vector3> starts = pathfinder.Getstartlocations();

            for (int c = 0; c < starts.Count; c++)
            {
                agents.Add(Instantiate(agentprefab, starts[c], Quaternion.Euler(0f, 0f, 0f)).GetComponent<Agent>());
                agents[c].Setid(c);
            }
            visionroutine = StartCoroutine("Checkvision");
        }

        private void OnDisable()
        {
            StopCoroutine(visionroutine);
        }
    }
}