using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RoombaBoss Movement 
public class Boss1Movement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 home;
    private bool playerInSight;
    private float startCycleTime;
    private SpriteRenderer sprite;
    private Animator animator;
    private enum BossState { ChargingUp, Charging, Paused }
    private BossState currentState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        home = transform.position;
        startCycleTime = -1;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = BossState.Paused;
    }

    // Update is called once per frame (fixed update for physics)
    void FixedUpdate()
    {
        switch (currentState)
        {
            case BossState.ChargingUp:
                HandleChargingUp();
                break;

            case BossState.Charging:
                HandleCharging();
                break;

            case BossState.Paused:
                HandlePaused();
                break;
        }
    }

    private void HandleChargingUp()
    {
        if (Time.time - startCycleTime <= 1.5f)
        {
            FacePlayer();
        }
        else
        {
            currentState = BossState.Charging;
            startCycleTime = Time.time; // Start the charging
        }
    }

    private void HandleCharging()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * 15; 

        // Check for collisions
        if (Physics2D.Raycast(transform.position, direction, 0.5f).collider)
        {
            rb.velocity = Vector2.zero; // Stop on collision
            currentState = BossState.Paused;
            startCycleTime = Time.time; // Start pause timer
        }
    }

    private void HandlePaused()
    {
        if (Time.time - startCycleTime >= 1.5f)
        {
            currentState = BossState.ChargingUp; // Restart cycle
            startCycleTime = Time.time; // Reset time
        }
    }

    private void FacePlayer()
    {
        Vector2 playerDirection = (player.transform.position - transform.position);

        if (playerDirection.x > 0 && sprite.flipX)
        {
            sprite.flipX = false;
        }
        else if (playerDirection.x < 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSight = true;
            if (currentState == BossState.Paused)
            {
                startCycleTime = Time.time; // Start charging up immediately if paused
                currentState = BossState.ChargingUp;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSight = false;
            currentState = BossState.Paused; // Pause if the player leaves sight
        }
    }
}
