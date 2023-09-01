using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Zombie : MonoBehaviour
{
    public Transform player;  
    public NavMeshAgent navMeshAgent;
    public GetOut getOut;
    public PlayerController playerController;
    public List<GameObject> zombies = new List<GameObject>();
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public float attackRange = 0.6f;
    private void Update()
    {

        if (player != null && playerController.isAlive)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < attackRange)
            {
                playerController.TakeDamage(1);
            }

            if (getOut.gate != null && getOut.gateIsOpen)
            {
                navMeshAgent.SetDestination(player.position);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

}


