using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttackLength = 0.3f;
    public float LastAttack = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LastAttack > 0)
        {
            LastAttack -= Time.deltaTime;
        } 
        else if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartAttack()
    {
        gameObject.SetActive(true);
        LastAttack = AttackLength;
    }
}
