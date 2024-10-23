using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        StartCoroutine(Instantiate());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().AddHealth(1);
            StartCoroutine(Collect());
        }
    }

    private IEnumerator Instantiate()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private IEnumerator Collect()
    {
        Animator.SetBool("Collected", true);
        GameManager.Collectibles += 1;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
