using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class Boss : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 home;
    private float startCycleTime;
    private SpriteRenderer sprite;
    private Animator animator;
    private bool firstCharge;
    Vector2 chargeDirection;
    private float chargeSpeed = 8;
    private enum BossState { ChargingUp, Charging, Paused, Waiting }
    private BossState currentState;

    void Awake()
    {
        GameManager.OnProgress += OnGameProgressChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnProgress -= OnGameProgressChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        home = transform.position;
        startCycleTime = -1;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = BossState.Waiting;
        firstCharge = true;
    }

    // Update is called once per frame
    
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
    //Anim 1: Rearing up faces player during this time
    private void HandleChargingUp()
    {
        if (Time.time - startCycleTime <= 0.5f)
        {
            FacePlayer();
            animator.SetBool("IsCharging", true);
        }
        else
        {
            currentState = BossState.Charging;
            startCycleTime = Time.time; // Start the charging
        }
    }

    private void HandleCharging()
    {
        //Fixes the charge curving
        if (firstCharge) 
        {
            startCycleTime = Time.time;
            firstCharge = false;
            chargeDirection = (player.transform.position - transform.position).normalized;

        }
        
        rb.velocity = chargeDirection * chargeSpeed;
        string tag = Physics2D.Raycast(transform.position, chargeDirection, 0.5f).collider?.gameObject?.tag;
        if (Time.time - startCycleTime > 1.0f)
        {
            animator.SetBool("IsCharging", false);
            currentState = BossState.Paused;
            startCycleTime = Time.time;
            // Check for collisions
        }
        else if ( tag != "Player" && tag !=null)
        {
            animator.SetBool("IsCharging", false);
            chargeDirection = Vector2.Reflect(chargeDirection, Physics2D.Raycast(transform.position, chargeDirection, 0.5f).normal);
            rb.velocity = chargeDirection * chargeSpeed; // Update velocity based on the new direction
        }
        if(Time.time - startCycleTime % .1f == 0)
        {
            rb.velocity = chargeDirection * 10000000;
        }
    }

    private void HandlePaused()
    {
        firstCharge = true;
        rb.velocity = Vector2.zero;
        if (Time.time - startCycleTime >= 1.5f)
        {
            currentState = BossState.ChargingUp; // Restart cycle
            startCycleTime = Time.time; // Reset time
        }
    }
    private void FacePlayer()
    {
        Vector2 playerDirection = (player.transform.position - transform.position);

        if (playerDirection.x > 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
        else if (playerDirection.x < 0 && sprite.flipX)
        {
            sprite.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(1);
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
            currentState = BossState.Paused; // Pause if the player leaves sight
        }
    }

    private void OnGameProgressChanged(Progress progress)
    {
        if (progress == Progress.BossRemaining)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<NPCHealth>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            currentState = BossState.Paused;
        }
    }
}
