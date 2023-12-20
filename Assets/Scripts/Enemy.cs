using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float distance;

    public int wanderChance;
    public float wanderDistance;

    Rigidbody rb;
    NavMeshAgent agent;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //transform.LookAt(target);
        //rb.velocity = transform.forward * speed;

        if(Vector3.Distance(rb.position, target.position) <= distance)
        {
            agent.destination = target.position;
        }
        else
        {
            if(Random.RandomRange(1, wanderChance) == 1)
            {
                var offset = Random.insideUnitSphere * wanderDistance;
                agent.destination = transform.position + offset;
            }
        }
        
    }
}
