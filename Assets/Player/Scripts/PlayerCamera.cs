using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject Player;
    public float HorizontalTrackingDistance;
    public float VerticalTrackingDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null) { 
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
}
