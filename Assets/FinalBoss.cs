using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FinalBoss : MonoBehaviour
{
    private GameObject player;
    private float oscillateAttack = 1f;
    private float oscillateAdd = -0.1f;
    public double attackInterval = 8;
    private DateTime lastAttack;
    private NPCHealth BossHealth;
    [SerializeField] GameObject projectile;
    private Rigidbody2D rb;
    private bool isActive = false;
    private Animator animator;
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
        BossHealth = GetComponent<NPCHealth>();
        lastAttack = DateTime.Now;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        // Boss stages
        if (isActive)
        {
            StartCoroutine(PulseForward());
            if (BossHealth.GetHealth() > 10)
            {
                if (DateTime.Now >= lastAttack.AddSeconds(attackInterval))
                {
                    StartCoroutine(OscillateAttack(30, 1f, 0.1f));
                    lastAttack = DateTime.Now;
                }
            }
            else if (BossHealth.GetHealth() == 10)
            {
                animator.SetBool("StageAttack", true);
                rb.constraints =  RigidbodyConstraints2D.FreezeAll;
                StopAllCoroutines();
                StartCoroutine(OscillateAttack(90, 10f, 0.1f));
            }
            else if (BossHealth.GetHealth() < 10 && BossHealth.GetHealth() > 5)
            {
                animator.SetBool("StageAttack", false);
                rb.constraints = RigidbodyConstraints2D.None;
                if (DateTime.Now >= lastAttack.AddSeconds(attackInterval))
                {
                    oscillateAttack = 1f;
                    StartCoroutine(OscillateAttack(50, 1f, 0.075f));
                    lastAttack = DateTime.Now;
                }
            }
            else if (BossHealth.GetHealth() == 5)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.SetBool("StageAttack", true);
                StopAllCoroutines();
                StartCoroutine(OscillateAttack(120, 10f, 0.075f));
            }
            else if (BossHealth.GetHealth() > 0 && BossHealth.GetHealth() < 5)
            {
                animator.SetBool("StageAttack", false);
                rb.constraints = RigidbodyConstraints2D.None;
                if (DateTime.Now >= lastAttack.AddSeconds(attackInterval))
                {
                    oscillateAttack = 1f;
                    StartCoroutine(OscillateAttack(70, 1f, 0.05f));
                    lastAttack = DateTime.Now;
                }
            }
        }

    }
    IEnumerator OscillateAttack(int bulletAmt, float rotation, float rotationInterval)
    {
        for (int i = 0; i < bulletAmt; i++) {
            Vector2 playerPos = player.transform.position - transform.position;

            // Oscillate between two angles
            if (oscillateAttack >= rotation) oscillateAdd = -rotationInterval;
            else if (oscillateAttack <= -rotation) oscillateAdd = rotationInterval;
            oscillateAttack += oscillateAdd;

            // Get the correct vector to instantiate by
            Vector2 playerOscillate = GetB(playerPos, oscillateAttack);
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            var manager = bullet.GetComponent<ProjectileManager>();
            manager.Fire(playerOscillate.normalized, 2);

            yield return new WaitForSeconds(.2f);
        }   
    }

    IEnumerator PulseForward()
    {
        animator.SetFloat("Speed", 1f);
        for (int i = 0; i < 8; i++)
        {
            rb.AddForce((player.transform.position - transform.position).normalized, ForceMode2D.Force);
            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// Gets the rotation matrix around the angle comparable to the player position
    /// </summary>
    /// <param name="angle">The angle of the vector to determine</param>
    /// <returns>The rotation matrix about the angle</returns>
    private float[,] GetRotationMatrix(float angle)
    {
        float[,] rotationMatrix = new float[2,2];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (i == j) rotationMatrix[i, j] = Mathf.Cos(angle) * Mathf.Rad2Deg;
                else if (i == 0) rotationMatrix[i, j] = -Mathf.Sin(angle) * Mathf.Rad2Deg;
                else rotationMatrix[i, j] = Mathf.Sin(angle) * Mathf.Rad2Deg;
            }
        }
        return rotationMatrix;
    }

    /// <summary>
    /// Gives you the dot product between the rotation matrix and the player position to determine
    /// angle B (the angle to fire a projectile at)
    /// </summary>
    /// <param name="A">The players position</param>
    /// <param name="angleRotate">the angle to rotate about</param>
    /// <returns>The vector at which to fire a projectile</returns>
    private Vector2 GetB(Vector2 A, float angleRotate)
    {
        Vector2 B = Vector2.zero;   
        float[,] rotationMatrix = GetRotationMatrix(angleRotate);
        for (int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
                B[i] += rotationMatrix[i, j] * A[j];   
        }
        return B;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isActive)
        {
           isActive = true;
        }
    }
}
