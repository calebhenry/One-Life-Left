using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCMovement : MonoBehaviour
{
    private GameObject player;
    public bool playerInSight;
    private Vector3 home;
    private Vector3 currDestination;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // Set home position as the position where enemy was instantiated on the map
        home = gameObject.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        // If player is in sightline move towards them, else return to home resting space
        if (playerInSight)
        {
            currDestination = player.transform.position;
        }
        else
        {
            currDestination = home;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = (currDestination - gameObject.transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInSight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInSight = false;
        }
    }
}
