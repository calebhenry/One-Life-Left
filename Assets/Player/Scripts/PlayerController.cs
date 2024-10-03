using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 5f;
    public float Offset = 0.1f;
    public ContactFilter2D ContactFilter;
    private Rigidbody2D RigidBody;
    private Animator animator;
    List<RaycastHit2D> Collisions = new();
    [Header("Attacks")]
    public float AttackCooldown = 0.6f;
    public GameObject AttackContainer;
    public Attack Attack;
    private float TimeSinceAttack = 0f;

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 positionChange = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            int numCollisons = RigidBody.Cast(Vector2.up, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                positionChange += Vector2.up;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            int numCollisons = RigidBody.Cast(Vector2.down, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                positionChange += Vector2.down;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            int numCollisons = RigidBody.Cast(Vector2.left, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                positionChange += Vector2.left;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            int numCollisons = RigidBody.Cast(Vector2.right, ContactFilter, Collisions, Offset + Time.deltaTime * Speed);
            if (numCollisons == 0)
            {
                positionChange += Vector2.right;
            }
        }
        Debug.Log(positionChange.normalized * Speed * Time.deltaTime);
        Debug.Log(positionChange.normalized);
        RigidBody.MovePosition(RigidBody.position + positionChange.normalized * Speed * Time.deltaTime);

        // Determine direction and angle to mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        AttackContainer.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Determine nearest vector direction (using dot product thanks Dr. Yee)
        float max = 0;
        int roundedDirection = 0;
        List<Vector2> list = new List<Vector2>() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
        foreach (var item in list) 
        {
            float dot = Vector2.Dot(direction, item);
            if (dot > max)
            {
                max = dot;
                roundedDirection = list.IndexOf(item); 
            }
        }
        animator.SetInteger("Direction", roundedDirection);

        // Start attack, dont allow till cooldown over
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
