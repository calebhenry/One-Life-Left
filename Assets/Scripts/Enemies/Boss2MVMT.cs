using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss2MVMT : MonoBehaviour
{
    public Tilemap tilemap;
    public float timeBetweenJumps = 5f;
    public float jumpMaxDist = 3f;
    private bool playerInSight;
    Time phaseTime;
    private enum BossState { Shooting, Dodging, Portal }
    private BossState currentState;
    private bool lastStateShooting;

    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.Shooting;
        lastStateShooting = true;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case BossState.Shooting:
                HandleShooting();
                break;

            case BossStateDodging:
                HandleDodging();
                break;

            case BossState.Portal:
                HandlePortal();
                break;
        }
    }

    private void HandleShooting()
    {

    }

    private void HandleDodging()
    {

    }

    private void HandlePortal()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(1);
            playerInSight = true;
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSight = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void Awake()
    {
        GameManager.OnProgress += OnGameProgressChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnProgress -= OnGameProgressChanged;
    }

    private void OnGameProgressChanged(Progress progress)
    {
        if (progress == Progress.BossRemaining)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<NPCHealth>().enabled = true;
            //GetComponent<NPCMovement>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
