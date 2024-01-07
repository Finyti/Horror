using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using System.Threading;
using Random = UnityEngine.Random;
using System.Reflection;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float distance;

    public int wanderChance;
    public float wanderDistance;

    public bool Seek = false;

    public PageManager pageManager;

    public Animator animator;

    public bool mirageCooldown = false;


    public GameObject Player;


    public GameObject SlenderPrefab;

    Rigidbody rb;
    NavMeshAgent agent;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator.Play("Idle");
    }

    void Update()
    {
        if (target == null) return;

        agent.speed = speed;
        //transform.LookAt(target);
        //rb.velocity = transform.forward * speed;

        if (Vector3.Distance(rb.position, target.position) <= distance)
        {
            if(pageManager.pageCount == 1)
            {
                animator.Play("Walk");
            }
            if (pageManager.pageCount >= 2)
            {
                animator.Play("Run");
            }
            Seek = true;
            agent.destination = target.position;
        }
        else
        {
            Seek = false;
            if (Random.RandomRange(1, wanderChance) == 1)
            {
                var offset = Random.insideUnitSphere * wanderDistance;
                agent.destination = transform.position + offset;
            }
            //else
            //{
            //    animator.Play("Idle");
            //}
        }

        if (pageManager.mirage && !mirageCooldown)
        {
            // mirageon a distance. How to give it correct direction.
            mirageCooldown = true;
            AsyncTimer.EventTimer ET = new AsyncTimer.EventTimer((pageManager.mirageReset * Random.RandomRange(2, 3)) * 10);
            ET.ProcessCompleted += MirageResetTimer;
            ET.StartProcess();
            GameObject clone = Instantiate(SlenderPrefab, new Vector3(Player.transform.position.x + (Player.transform.eulerAngles.y/180 * 5), 
                Player.transform.position.y, Player.transform.position.z + (Player.transform.eulerAngles.y / 180 * 5)), Player.transform.rotation);
            if (Player.transform.rotation.y > 0)
            {
                clone.transform.eulerAngles = new Vector3(Player.transform.eulerAngles.x, Player.transform.eulerAngles.y - 180, Player.transform.eulerAngles.z);
            }
            else
            {
                clone.transform.eulerAngles = new Vector3(Player.transform.eulerAngles.x, Player.transform.eulerAngles.y  - 180, Player.transform.eulerAngles.z);
            }

            print(Player.transform.eulerAngles.y);
            Destroy(clone, 10);
        }
        
    }
    public void MirageResetTimer(object sender, EventArgs e)
    {
        mirageCooldown = false;
        print("f");
    }
}
