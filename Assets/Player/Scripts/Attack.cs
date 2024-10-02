using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttackLength = 0.3f;
    public int AttackDamage = 1;
    float LastAttack = 0;
    Collider2D Collider;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LastAttack > 0)
        {
            LastAttack -= Time.deltaTime;
        } 
        else if (gameObject.activeSelf)
        {
            Collider.enabled = false;
        }
    }

    public void StartAttack()
    {
        Collider.enabled = true;
        LastAttack = AttackLength;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack" + collision.tag);
        if(collision.tag == "Enemy")
        {
            Debug.Log("Attack2");
            HealthManager health = collision.GetComponent<HealthManager>();
            health.TakeDamage(1);
        }
    }
}
