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
    public bool teleportCooldown = false;


    public GameObject Player;


    public GameObject SlenderPrefab;


    AudioSource audioSource;
    public AudioClip Teleport;

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
        
        if(Seek == false || Vector3.Distance(rb.position, target.position) > 20)
        {
            if (pageManager.teleport && !teleportCooldown)
            {
                teleportCooldown = true;
                AsyncTimer.EventTimer ET = new AsyncTimer.EventTimer((pageManager.teleportReset * Random.RandomRange(2, 3)) * 1000);
                ET.ProcessCompleted += TeleportResetTimer;
                ET.StartProcess();
                DoEnemyPosition();

                audioSource = GetComponent<AudioSource>();

                audioSource.PlayOneShot(Teleport);
            }
           
        }

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
            AsyncTimer.EventTimer ET = new AsyncTimer.EventTimer((pageManager.mirageReset * Random.RandomRange(2, 3)) * 1000);
            ET.ProcessCompleted += MirageResetTimer;
            ET.StartProcess();
            GameObject clone = Instantiate(SlenderPrefab, new Vector3(Player.transform.position.x + Player.transform.forward.x * 20, 
                Player.transform.forward.y + 15, Player.transform.position.z + Player.transform.forward.z * 20), Player.transform.rotation);
            DoClonePosition(clone);
            if (clone.transform.position.y >= 4)
            {
                clone.transform.position += Vector3.up * 15;
                DoClonePosition(clone);
            }

            Destroy(clone, 0.5f);
        }




    }
    public void DoClonePosition(GameObject clone)
    {
        clone.transform.position += Vector3.right * Random.RandomRange(-20, 20);
        Ray ray = new Ray(clone.transform.position, Vector3.down * 100);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - hitData.distance, clone.transform.position.z);
        clone.transform.LookAt(Player.transform.position);

    }

    public void DoEnemyPosition()
    {
        transform.position = Player.transform.position;
        transform.position += Vector3.up * 5;


        transform.position += new Vector3(RandomPosNeg(15, 10, true), 0, RandomPosNeg(15, 10, true));
        
        //transform.position += Vector3.forward * 5;

        Ray ray = new Ray(transform.position, Vector3.down * 100);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        transform.position -= Vector3.down * hitData.distance;



    }
    public void MirageResetTimer(object sender, EventArgs e)
    {
        mirageCooldown = false;
    }

    public void TeleportResetTimer(object sender, EventArgs e)
    {
        teleportCooldown = false;
    }

    public int RandomPosNeg(int min, int max, bool multiMode)
    {
        if(multiMode)
        {
            int i = Random.RandomRange(1, 3);
            print(i);
            if(i == 1)
            {
                return Random.RandomRange(-min, -max);
            }
            else
            {
                return Random.RandomRange(min, max);
            }

        }
        else
        {
            return Random.RandomRange(min, max);
        }
    }
}
