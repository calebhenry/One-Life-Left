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
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.8f, 0.2f);
        Rigidbody.velocity = direction * Speed;
        Deflected = true;
    }

    private void ProcessCollisionNotDeflected(Collider2D collision)
    {
        List<string> IgnoreTags = new() { "Enemy", "Boss", "Attack", "Projectile" };
        switch (collision.tag)
        {
            case "Player":
                PlayerHealth health;
                if (collision.TryGetComponent<PlayerHealth>(out health))
                {
                    health.TakeDamage(1);
                }
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                break;
            case string s when IgnoreTags.Contains(s):
                break;
            case "Breakable":
                Breakable breakable = collision.GetComponent<Breakable>();
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                breakable.Break();
                break;
            default:
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                break;
        }
    }

    private void ProcessCollisionDeflected(Collider2D collision)
    {
        List<string> IgnoreTags = new() { "Player", "Attack", "Projectile" };
        switch (collision.tag)
        {
            case "Enemy":
                collision.GetComponent<NPCHealth>().TakeDamage(1);
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                break;
            case "Boss":
                collision.GetComponent<NPCHealth>().TakeDamage(1);
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                break;
            case string s when IgnoreTags.Contains(s):
                break;
            case "Breakable":
                Breakable breakable = collision.GetComponent<Breakable>();
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                breakable.Break();
                break;
            default:
                gameObject.GetComponent<NPCHealth>().TakeDamage(1);
                break;
        }
    }
}
