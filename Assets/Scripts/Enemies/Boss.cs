using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameProgressChanged(Progress progress)
    {
        if (progress == Progress.BossRemaining)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<NPCHealth>().enabled = true;
            GetComponent<NPCMovement>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
