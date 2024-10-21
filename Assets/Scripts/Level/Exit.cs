using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public static event Action<bool> OnExitAvailible;

    private BoxCollider2D Collider;
    private bool AbleToExit = false;
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
        Collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && AbleToExit) 
        {
            GameManager.Instance.OnExit();
        }
    }

    private void OnGameProgressChanged(Progress progress)
    {
        if (progress == Progress.Complete)
        {
            Collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AbleToExit = true;
            OnExitAvailible?.Invoke(AbleToExit);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AbleToExit = false;
            OnExitAvailible?.Invoke(AbleToExit);
        }
    }
}
