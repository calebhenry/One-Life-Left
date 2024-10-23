using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    private const int testDespawn = 1;

    public bool burst;
    public double attackInterval;
    public int attackAmt;
    public float ProjectileSpeed = 3;
    [SerializeField] GameObject projectile;
    // Start is called before the first frame update
    private GameObject player;
    private DateTime lastAttack;
    private bool playerInSight;
    void Start()
    {
        player = GameObject.Find("Player");
        lastAttack = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = gameObject.GetComponent<NPCMovement>().playerInSight;
        if (DateTime.Now >= lastAttack.AddSeconds(attackInterval) && burst && playerInSight)
        {
            StartCoroutine(BurstAttack());
            lastAttack = DateTime.Now;
        }
        else if (!playerInSight) 
        {
            StopAllCoroutines();
            // Leave this up to you kat, nat, or caleb for what you want to do with boss fights or other enemies
        }
    }
    /// <summary>
    /// Attacks in bursts depending on the projectile amount
    /// </summary>
    /// <returns>Nothing</returns>
    IEnumerator BurstAttack()
    {
        for (int i = 0; i < attackAmt; i++)
        {
            yield return new WaitForSeconds(.25f);
            GameObject bullet = Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            var manager = bullet.GetComponent<ProjectileManager>();
            manager.Fire((player.transform.position - gameObject.transform.position).normalized, ProjectileSpeed);
        }
    }
}
