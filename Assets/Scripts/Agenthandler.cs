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
        [SerializeField] private int visionrange = 5;

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
            else if(pathtrigger == 3)
            {
                //empty path
            }
            return pathfinder.Astar(agents[id].transform.position, target);
        }

        public void Checkvision(int id)
        {
            List<Vector3> target = new List<Vector3>();
            for (int c = 0; c < agents.Count; c++)
            {
                if (c != id)
                    if (Vector3.Distance(agents[id].transform.position, agents[c].transform.position) <
                        visionrange)
                        target.Add(agents[c].transform.position);
            }
            //raycast
            //return closest in vision
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
    }
}