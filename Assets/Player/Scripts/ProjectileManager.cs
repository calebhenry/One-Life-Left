using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        StartCoroutine(DelayedDestroyTesting());
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Some type of parry logic would go here where it would reverse the velocity
        if (collision.tag == "Player")
        {
            Debug.Log("Hit player!");
            Player.GetComponent<HealthManager>().TakeDamage(1);
        } 
        if (collision.tag != "Enemy")
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
