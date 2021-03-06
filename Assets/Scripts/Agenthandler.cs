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
        [HideInInspector] private bool dodelete = false;
        [HideInInspector] private Vector3 distance;
        [HideInInspector] private float offset = 0.2f;

        [SerializeField] private GameObject agentprefab;
        [SerializeField] private float visionrange = 5.0f;
        [SerializeField] private float visionupdaterate = 1.0f;
        [SerializeField] private GameObject bulletprefab;

        public List<Vector3> Requestpath(int id, int pathtrigger = 0)
        {
            Vector3 target = Getfartarget();

            if (pathtrigger == 1)
            {
                for (int c = 0; c < agents.Count; c++)
                {
                    if (agents[c] != null && c != id)
                        if (Vector3.Distance(agents[id].transform.position, agents[c].transform.position) <
                            Vector3.Distance(target, agents[c].transform.position))
                            target = agents[c].transform.position;
                }
            }
            else if (pathtrigger == 2)
            {
                List<Vector3> healths = pathfinder.Gethealthlocations();
                for (int c = 0; c < healths.Count; c++)
                {
                    if (Vector3.Distance(agents[id].transform.position, healths[c]) <
                        Vector3.Distance(agents[id].transform.position, target))
                        target = healths[c];
                }
            }
            else if (pathtrigger == 3)
            {
                List<Vector3> ammos = pathfinder.Getammolocations();
                for (int c = 0; c < ammos.Count; c++)
                {
                    if (Vector3.Distance(agents[id].transform.position, ammos[c]) <
                        Vector3.Distance(agents[id].transform.position, target))
                        target = ammos[c];
                }
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
                        if (agents[c] != null && agents[q] != null)
                            if (Vector3.Distance(agents[c].transform.position, agents[q].transform.position) < visionrange)
                                sightpotential.Add(new Vector2Int(c, q));

                RaycastHit2D hit;
                Vector3 direction;

                for (int c = 0; c < sightpotential.Count; c++)
                {
                    agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = false;

                    direction = (agents[sightpotential[c].y].transform.position - agents[sightpotential[c].x].transform.position).normalized;
                    Vector3 tangent = new Vector3(-direction.y, direction.x, 0);
                    hit = Physics2D.Raycast(agents[sightpotential[c].x].transform.position + tangent * offset, direction, visionrange);

                    Debug.DrawRay(agents[sightpotential[c].x].transform.position + tangent * offset, direction, Color.cyan, 1f);
                    Debug.DrawRay(agents[sightpotential[c].x].transform.position + tangent * -offset, direction, Color.red, 1f);

                    agents[sightpotential[c].x].GetComponent<Collider2D>().enabled = true;

                    if (hit.collider)
                        if (hit.collider.CompareTag("Agent") &&
                            Physics2D.Raycast(agents[sightpotential[c].x].transform.position + tangent * -offset, direction, visionrange).collider.CompareTag("Agent"))
                        {
                            agents[sightpotential[c].x].Receivevisual(agents[sightpotential[c].y].transform.position);
                            agents[sightpotential[c].y].Receivevisual(agents[sightpotential[c].x].transform.position);
                        }
                }
                yield return new WaitForSeconds(visionupdaterate);
            }
        }

        public void Dead(int id)
        {
            for(int c = 0; c < agents.Count; c++)
                if(c != id)
                    if(agents[c] != null)
                        agents[c].Targetdown(agents[id].transform.position);
            dodelete = true;
        }

        public Vector3 Getfartarget()
        {
            return distance;
        }

        private void Awake()
        {
            pathfinder = GetComponentInParent<Pather>();

            offset = bulletprefab.transform.localScale.x;
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

            distance = GameObject.Find("Distantobject").transform.position;
        }

        private void OnDisable()
        {
            StopCoroutine(visionroutine);
        }

        private void LateUpdate()
        {
            if (dodelete)
                for (int c = 0; c < agents.Count; c++)
                    if (agents[c] != null && !agents[c].isActiveAndEnabled)
                        Destroy(agents[c].gameObject);
        }
    }
}