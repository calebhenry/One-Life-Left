using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float AttackCooldown = 0.6f;
    public float Offset = 0.1f;
    private float TimeSinceAttack = 0f;
    public GameObject AttackContainer;
    public Attack Attack;
    public Rigidbody2D RB;
    public ContactFilter2D ContactFilter;
    List<RaycastHit2D> Collisions = new();

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPostion = RB.position;
        if (Input.GetKey(KeyCode.W))
        {
            int numCollisons = RB.Cast(Vector2.up, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                newPostion += Vector2.up * Speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            int numCollisons = RB.Cast(Vector2.down, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                newPostion += Vector2.down * Speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            int numCollisons = RB.Cast(Vector2.left, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                newPostion += Vector2.left * Speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            int numCollisons = RB.Cast(Vector2.right, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                newPostion += Vector2.right * Speed * Time.deltaTime;
            }
        }
        RB.MovePosition(newPostion);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        AttackContainer.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0) && TimeSinceAttack >= AttackCooldown) 
        {
            Attack.StartAttack();
            TimeSinceAttack = 0;
        }
        else if (TimeSinceAttack < AttackCooldown)
        {
            TimeSinceAttack += Time.deltaTime;
        }
    }
}
