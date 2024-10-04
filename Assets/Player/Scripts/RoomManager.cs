using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RoomManager : MonoBehaviour
{
    public int enemyAmount;
    public double spawnInterval;
    [SerializeField] GameObject enemy;
    private DateTime lastSpawn;
    // Start is called before the first frame update
    [SerializeField] GameObject playSpace;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.Now >= lastSpawn.AddSeconds(spawnInterval) && enemyAmount > 0)
        {
            Instantiate(enemy, gameObject.transform.position , gameObject.transform.rotation);
            enemyAmount--;
            lastSpawn = DateTime.Now;
        }
    }
}
