using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody2D Rigidbody;
    private bool Deflected;
    private float Speed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(DelayedDestroyTesting());
        Deflected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Deflected)
        {
            ProcessCollisionNotDeflected(collision);
        }
        else
        {
            ProcessCollisionDeflected(collision);
        }
    }

    public void Fire(Vector2 direction, float speed)
    {
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
        Rigidbody.velocity = direction * speed;
        Speed = speed;
    }

    public void Deflect(Vector2 direction)
    {
        Rigidbody.velocity = direction * Speed;
        Deflected = true;
    }

    private void ProcessCollisionNotDeflected(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.GetComponent<HealthManager>().TakeDamage(1);
        }
        if (collision.tag != "Enemy" && collision.tag != "Attack" && collision.tag != "Projectile")
        {
            gameObject.GetComponent<HealthManager>().TakeDamage(1);
        }
    }

    private void ProcessCollisionDeflected(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Hit Enemy!");
            collision.GetComponent<HealthManager>().TakeDamage(1);
        }
        if (collision.tag != "Player" && collision.tag != "Attack" && collision.tag != "Projectile")
        {
            gameObject.GetComponent<HealthManager>().TakeDamage(1);
        }
    }

    IEnumerator DelayedDestroyTesting()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }
}
