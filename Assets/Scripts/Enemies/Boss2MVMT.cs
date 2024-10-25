using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class Boss2MVMT : MonoBehaviour
{
    [SerializeField]
    GameObject Projectile;
    [SerializeField]
    GameObject BurstProjectile;
    [SerializeField]
    GameObject Enemy;
    [SerializeField]
    private float Speed = 1;
    [Header("Attacks")]
    [SerializeField]
    private float AttackInterval;
    [SerializeField]
    private int NumberOfBursts = 4;
    [SerializeField]
    private float AttackPause = 0.75f;
    [SerializeField]
    private int LengthOfBeam = 50;
    [SerializeField]
    private int BeamBurstInterval = 15; // How often a beam shot bursts
    private GameObject Player;
    private bool playerInSight;
    private bool Ready = true;
    private bool Moving = false;
    private Rigidbody2D rb;
    private NPCHealth BossHealth;
    private Animator animator;
    private SpriteRenderer sprite;
    private float TimeSinceAttack;
    private Vector3 Destination;
    private List<Transform> targets;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        BossHealth = GetComponent<NPCHealth>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        targets = GameObject.Find("Destinations").GetComponentsInChildren<Transform>().ToList();
        TimeSinceAttack = AttackInterval - 1;
    }

    void FixedUpdate()
    {
        if (playerInSight)
        {
            FacePlayer();
            if (TimeSinceAttack >= AttackInterval && Ready)
            {
                TimeSinceAttack = 0;
                StartCoroutine(BurstAttack(NumberOfBursts));
            }
            if (!Moving)
            {
                StartCoroutine(Move());
            }
            else if (Vector2.Distance(Destination, transform.position) > 0.5)
            {
                var direction = Destination - transform.position;
                rb.MovePosition((Vector3)rb.position + Speed * Time.deltaTime * direction.normalized);
            }
            TimeSinceAttack += Time.deltaTime;
        }
    }

    IEnumerator Move()
    {
        Moving = true;
        var random = new System.Random();
        Destination = targets.Where(t => t.position != Destination).ToArray()[random.Next(targets.Count - 2)].position;
        while (Vector2.Distance(Destination, transform.position) > 0.5)
        {
            yield return new WaitForSeconds(0.2f);
        }
        StartCoroutine(BeamAttack());
    }

    IEnumerator BurstAttack(int numProjectiles)
    {
        var random = new System.Random();
        animator.SetBool("Attacking", true);
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject projectile = Instantiate(BurstProjectile, gameObject.transform.position, Quaternion.identity);
            ProjectileManager manager;
            if (projectile.TryGetComponent<ProjectileManager>(out manager))
            {
                manager.BurstTime = (float) (manager.BurstTime + (0.2 * random.Next(5)));
                manager.Fire((Player.transform.position - transform.position).normalized, 4);
            }
            yield return new WaitForSeconds(0.2f);
        }
        animator.SetBool("Attacking", false);
    }

    IEnumerator BeamAttack()
    {
        var random = new System.Random();
        Ready = false;
        StopCoroutine(BurstAttack(NumberOfBursts));
        yield return new WaitForSeconds(AttackPause);
        animator.SetBool("Attacking", true);
        for (int i = 0; i < LengthOfBeam; i++)
        {
            GameObject projectile;
            if (i % BeamBurstInterval == 0)
            {
                projectile = Instantiate(BurstProjectile, gameObject.transform.position, Quaternion.identity);
            }
            else
            {
                projectile = Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
            }
            ProjectileManager manager;
            if (projectile.TryGetComponent<ProjectileManager>(out manager))
            {
                manager.Fire((Player.transform.position - transform.position).normalized, 4);
            }
            yield return new WaitForSeconds(0.1f);
        }
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(AttackPause);
        Instantiate(Enemy, targets.Where(t => t.position != Destination).ToArray()[random.Next(targets.Count - 2)].position, Quaternion.identity);
        Ready = true;
        Moving = false;
        yield return null;
    }

    private void FacePlayer()
    {
        Vector2 playerDirection = (Player.transform.position - transform.position);

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
            playerInSight = true;
        }
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
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
