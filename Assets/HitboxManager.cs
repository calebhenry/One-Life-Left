using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Player.transform.position - transform.position);
    }


}
