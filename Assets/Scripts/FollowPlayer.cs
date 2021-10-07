using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private Transform FollowTarget;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        FollowTarget = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(FollowTarget.position);
    }
}
