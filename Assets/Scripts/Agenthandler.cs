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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                GameObject go = agentprefab[0];

                agents.Add(Instantiate(go, new Vector3(-50f, -50f, 0), Quaternion.Euler(0f, 0f, 0f)).GetComponent<Agent>());
                agents[agents.Count - 1].Givepath(pathfinder.Astar());
            }
        }

        private void Awake()
        {
            pathfinder = GetComponentInParent<Pather>();
        }
    }
}