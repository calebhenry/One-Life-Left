using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 5;
    private Animator Animator;
    private Rigidbody2D RB;
    private SpriteRenderer Sprite;
    private bool CanDamage = true;

    //for when bosses are put in
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (gameObject.tag == "Enemy" || gameObject.tag == "Boss")
        {
            Animator = GetComponent<Animator>();
            RB = GetComponent<Rigidbody2D>();
        } 
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            //TODO: make sure bosses have sound as well, will trigger on demise
            audioSource.Play();
            if (gameObject.tag == "Enemy")
            {
                GameManager.Instance.EnemyDestroyed();
                StartCoroutine(Death());
            }
            else if (gameObject.tag == "Boss")
            {
                GameManager.Instance.OnComplete();
                StartCoroutine(BossDeath());
            }  
            else
            {
                if (gameObject.tag == "Boss")
                    GameManager.Instance.OnComplete();
                DestroyImmediate(gameObject);
            }             
        }
    }
    /// <summary>
    /// Decrements the object's health by a specified integer
    /// </summary>
    /// <param name="damage">Amount to decrement by</param>
    public void TakeDamage(int damage)
    {
        if (CanDamage) 
        {
            health -= damage;
            if (health > 0)
            {
                StartCoroutine(ShowDamage());
            }

            Debug.Log(GetComponent<Collider2D>().tag + " took damage, remaining health is " + health);
        }
    }


    public int GetHealth()
    {
        return health;
    }

    private IEnumerator Death()
    {
        Sprite.color = Color.white;
        Animator.SetBool("Dead", true);
        gameObject.GetComponent<NPCMovement>().enabled = false;
        gameObject.GetComponent<RangedAttack>().StopAllCoroutines();
        gameObject.GetComponent<RangedAttack>().enabled = false;
        RB.velocity = Vector3.zero;
        RB.totalForce = Vector2.zero;
        RB.gravityScale = 0f;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Hitbox").GetComponent<PlayerHealth>().AddHealth(1);
        Destroy(gameObject);
    }

    private IEnumerator BossDeath()
    {
        Sprite.color = Color.white;
        Animator.SetBool("Dead", true);
        gameObject.GetComponent<Boss>().enabled = false;
        RB.velocity = Vector3.zero;
        RB.totalForce = Vector2.zero;
        RB.gravityScale = 0f;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator ShowDamage()
    {
        Sprite.color = Color.red;
        CanDamage = false;
        yield return new WaitForSeconds(0.2f);
        Sprite.color = Color.white;
        CanDamage = true;
    }
}
