using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private AgentAuthoring agentAuth;
    // Start is called before the first frame update
    void Start()
    {
        agentAuth = GetComponent<AgentAuthoring>();
        if (destination == null)
        {
            destination = GameObject.FindWithTag("Destination").transform;
        }
        agentAuth.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
