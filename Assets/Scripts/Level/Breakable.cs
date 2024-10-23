using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public bool ContainsCollectible = false;
    public GameObject Collectible;
    private Animator Animator;
    private bool IsBroken;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        IsBroken = false;
    }

    public void Break()
    {
        Debug.Log("break");
        if (!IsBroken)
        {
            Animator.SetBool("Broken", true);
            IsBroken = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
        if(ContainsCollectible) 
            Instantiate(Collectible, transform.position, Quaternion.identity);
    }
}
