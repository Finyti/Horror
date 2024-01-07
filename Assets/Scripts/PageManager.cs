using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{

    public int pageCount = 0;
    public Enemy enemy;
    public bool mirage = false;
    public bool teleport = false;
    public int mirageReset = 10;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        pageCount++;

        if(pageCount == 1)
        {
            enemy.target = gameObject.transform;
        }
        if (pageCount == 2)
        {
            enemy.speed *= 1.5f;
        }
        if (pageCount == 3)
        {
            enemy.distance *= 1.5f;
            enemy.wanderDistance *= 1.5f;
        }
        if (pageCount == 4)
        {
            mirage = true;
        }
        if (pageCount == 5)
        {
            mirageReset = 10;
            enemy.speed *= 1.4f;
            enemy.distance *= 1.2f;
            enemy.wanderDistance *= 1.5f;
        }
        if (pageCount == 6)
        {
            teleport = true;
        }
    }
}
