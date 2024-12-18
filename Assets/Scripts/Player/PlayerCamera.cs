using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour
{
    public float HorizontalTrackingDistance;
    public float VerticalTrackingDistance;
    private GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var playerPosition = Player.transform.position;
        var cameraPosition = gameObject.transform.position;

        if (Mathf.Abs(cameraPosition.x - playerPosition.x) > HorizontalTrackingDistance)
        {
            var direction = Mathf.Sign((playerPosition - cameraPosition).x);
            gameObject.transform.Translate(new Vector3(Mathf.Abs(playerPosition.x - cameraPosition.x) - HorizontalTrackingDistance, 0) * direction);
        }

        if (Mathf.Abs(cameraPosition.y - playerPosition.y) > VerticalTrackingDistance)
        {
            var direction = Mathf.Sign((playerPosition - cameraPosition).y);
            gameObject.transform.Translate(new Vector3(0, Mathf.Abs(playerPosition.y - cameraPosition.y) - VerticalTrackingDistance) * direction);
        }
    }
}
