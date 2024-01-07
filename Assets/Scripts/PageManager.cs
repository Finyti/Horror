using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{

    public int pageCount = 0;
    public Enemy enemy;
    public bool mirage = false;
    public bool teleport = false;
    public int mirageReset = 5;
    public int teleportReset = 20;


    AudioSource audioSource;
    public AudioClip PageGrab;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        pageCount++;

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(PageGrab);

        if (pageCount == 1)
        {
            enemy.target = gameObject.transform;
            enemy.speed *= 1.2f;
        }
        if (pageCount == 2)
        {
            mirage = true;

        }
        if (pageCount == 3)
        {
            enemy.speed *= 1.2f;
            mirageReset--;
        }
        if (pageCount == 4)
        {
            teleport = true;
            mirageReset--;

        }
        if (pageCount == 5)
        {
            enemy.distance *= 1.1f;
            enemy.wanderDistance *= 1.4f;
            mirageReset--;
            teleportReset = 15;

        }
        if (pageCount == 6)
        {
            mirageReset = 10;
            enemy.speed *= 1.2f;
            enemy.distance *= 1.1f;
            enemy.wanderDistance *= 1.4f;
            teleportReset = 10;
        }
    }
}
