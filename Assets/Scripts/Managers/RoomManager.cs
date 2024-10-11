using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.ParticleSystem;

public class RoomManager : MonoBehaviour
{
    public int enemyAmount;
    public double spawnInterval;
    [SerializeField] GameObject enemy;
    private DateTime lastSpawn;
    private System.Random randomLocationSpawner = new System.Random();
    // Start is called before the first frame update
    [SerializeField] GameObject playSpace;
    void Start()
    {
        // Compress bounds to playable space
        playSpace.GetComponent<Tilemap>().CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.Now >= lastSpawn.AddSeconds(spawnInterval) && enemyAmount > 0)
        {
            BoundsInt bounds = playSpace.GetComponent<Tilemap>().cellBounds;
            Vector2 spawnPos = new Vector2(randomLocationSpawner.Next(bounds.x+1, bounds.xMax-1), 
                                           randomLocationSpawner.Next(bounds.y+1, bounds.yMax-1));
            Instantiate(enemy, spawnPos, gameObject.transform.rotation);
            enemyAmount--;
            lastSpawn = DateTime.Now;
        }
    }
}
