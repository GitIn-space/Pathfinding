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

        [SerializeField] private GameObject[] agentprefab;
        [SerializeField] private float visionrange = 5f;

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

        public void Checkvision()
        {
            List<Vector2Int> sightpotential = new List<Vector2Int>();
            for (int c = 0; c < agents.Count - 1; c++)
                for (int q = c + 1; q < agents.Count; q++)
                    if (Vector3.Distance(agents[c].transform.position, agents[q].transform.position) < visionrange)
                    {
                        sightpotential.Add(new Vector2Int(c, q));
                        Debug.DrawLine(agents[c].transform.position, agents[q].transform.position, Color.red, 1f);
                    }

            for (int c = 0; c < sightpotential.Count; c++)
            {
                agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = false;

                RaycastHit2D hit = Physics2D.Raycast(agents[sightpotential[c].x].transform.position,
                    agents[sightpotential[c].y].transform.position, visionrange);

                agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = true;

                if (hit.collider)
                {
                    Debug.Log(hit.collider.tag);
                    if ((hit.collider.CompareTag("Agent")))
                    {
                        agents[sightpotential[c].x].Receivevisual(agents[sightpotential[c].y].transform.position);
                        agents[sightpotential[c].y].Receivevisual(agents[sightpotential[c].x].transform.position);
                        Debug.DrawLine(agents[sightpotential[c].x].transform.position, agents[sightpotential[c].y].transform.position, Color.cyan, 60f);
                        //remove from lateupdate, make 1s coprocess
                    }
                }
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
                agents.Add(Instantiate(agentprefab[0], starts[c], Quaternion.Euler(0f, 0f, 0f)).GetComponent<Agent>());
                agents[c].Setid(c);
            }
        }

        private void LateUpdate()
        {
            Checkvision();
        }
    }
}