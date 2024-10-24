using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator Animator;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.Play();
        Animator.SetBool("Collected", true);
        GameManager.Collectibles += 1;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
