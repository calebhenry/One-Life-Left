using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private int NewLocationX = 0;
    [SerializeField]
    private int NewLocationY = 0;
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
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.transform.parent.GetChild(0);
            Debug.Log(new Vector3(NewLocationX, NewLocationY, 0) - player.transform.position);
            Debug.Log(player.transform.position);
            player.transform.Translate(new Vector3(NewLocationX, NewLocationY, 0) - player.transform.position);
        }
    }

    private void OnGameProgressChanged(Progress progress)
    {
        if (progress == Progress.BossRemaining)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
