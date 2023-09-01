using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Worker: MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform firstTarget;
    public Transform secondTarget;
    private Transform currentTarget;
    public float targetDistance = 2f;
    //public StartMetal startMetal;

    private void Start()
    {
        currentTarget = firstTarget;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) < targetDistance)
        {
           

                    // Move to the next target after interaction
                    currentTarget = currentTarget == firstTarget ? secondTarget : firstTarget;
                    agent.SetDestination(currentTarget.position);
         }
    }
}
    




