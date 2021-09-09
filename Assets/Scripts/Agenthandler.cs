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

        public List<Vector3> Requestpath(int id)
        {
            Vector3 temp = Vector3.zero;

            for(int c = 0; c < agents.Count; c++)
            {
                if(c != id)
                    if(Mathf.Abs(agents[id].transform.position - agents[c].transform.position))
            }

            return pathfinder.Astar(agents[id].transform);
        }

        private void Update()
        {
            /*if(Input.GetKeyDown(KeyCode.S))
            {
                GameObject go = agentprefab[0];

                agents.Add(Instantiate(go, new Vector3(0f, 0f, 0), Quaternion.Euler(0f, 0f, 0f)).GetComponent<Agent>());
                agents[agents.Count - 1].Givepath(pathfinder.Astar());
            }*/
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