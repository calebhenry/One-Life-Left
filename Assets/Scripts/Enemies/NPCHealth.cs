using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            if (gameObject.tag == "Enemy")
                GameManager.Instance.EnemyDestroyed();
            if (gameObject.tag == "Boss")
                GameManager.Instance.OnComplete();
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Decrements the object's health by a specified integer
    /// </summary>
    /// <param name="damage">Amount to decrement by</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(GetComponent<Collider2D>().tag + " took damage, remaining health is " + health);
    }
}
