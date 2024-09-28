using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float AttackCooldown = 0.6f;
    private float TimeSinceAttack = 0f;
    public GameObject PlayerRotation;
    public UnityEvent Attack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlayerRotation.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        if (Input.GetMouseButtonDown(0) && TimeSinceAttack >= AttackCooldown) 
        {
            Attack.Invoke();
            TimeSinceAttack = 0;
        }
        else if (TimeSinceAttack < AttackCooldown)
        {
            TimeSinceAttack += Time.deltaTime;
        }
    }
}
