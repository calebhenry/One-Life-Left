using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdatePlayerHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameManager.Instance.OnFail();
        }
    }
    /// <summary>
    /// Decrements the object's health by a specified integer
    /// </summary>
    /// <param name="damage">Amount to decrement by</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        GameManager.Instance.UpdatePlayerHealth(health);
        Debug.Log(GetComponent<Collider2D>().tag + " took damage, remaining health is " + health);
    }
    public void AddHealth(int health)
    {
        this.health += health;
        GameManager.Instance.UpdatePlayerHealth(this.health);
    }
}
