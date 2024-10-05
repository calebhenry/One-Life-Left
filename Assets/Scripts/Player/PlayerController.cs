using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 5f;
    public float DashSpeed = 30f;
    public float Offset = 0.1f;
    public float DashTime = 0.2f;
    public float DashEnergyCost = 5f;
    public ContactFilter2D ContactFilter;
    private Rigidbody2D RigidBody;
    private Animator Animator;
    List<RaycastHit2D> Collisions = new();
    [Header("Attacks")]
    public float AttackCooldown = 0.6f;
    public GameObject AttackContainer;
    public Attack Attack;

    // State managment
    private float TimeSinceAttack = 0f;
    private float TimeSinceDash = 0f;
    private Vector2 DashDirection;
    private bool Dashing = false;
    private float Energy = 0;

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Dashing)
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
            RigidBody.MovePosition(RigidBody.position + positionChange.normalized * Speed * Time.deltaTime);

            // Determine direction and angle to mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 direction = (mousePos - transform.position).normalized;

            if (Input.GetKey(KeyCode.Space) && !Dashing && Energy > DashEnergyCost)
            {
                Dashing = true;
                DashDirection = direction;
                Energy -= DashEnergyCost;
            }

            if (TimeSinceAttack >= AttackCooldown)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                AttackContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Determine nearest vector direction (using dot product thanks Dr. Yee)
            float max = 0;
            int roundedDirection = 0;
            List<Vector2> list = new List<Vector2>() { Vector2.right, Vector2.left };
            foreach (var item in list)
            {
                float dot = Vector2.Dot(direction, item);
                if (dot > max)
                {
                    max = dot;
                    roundedDirection = list.IndexOf(item);
                }
            }
            Animator.SetInteger("Direction", roundedDirection);

            // Start attack, dont allow till cooldown over
            if (Input.GetMouseButton(0) && TimeSinceAttack >= AttackCooldown)
            {
                Attack.StartAttack(direction);
                TimeSinceAttack = 0;
                Energy += 1;
            }
            else if (TimeSinceAttack < AttackCooldown)
            {
                TimeSinceAttack += Time.deltaTime;
            }
        }
        else
        {
            int numCollisons = RigidBody.Cast(DashDirection, ContactFilter, Collisions, Offset + Time.deltaTime *DashSpeed);
            if (numCollisons == 0)
            {
                RigidBody.MovePosition(RigidBody.position + DashDirection * DashSpeed * Time.deltaTime);
                TimeSinceDash += Time.deltaTime;
                if (TimeSinceDash >= DashTime)
                {
                    Dashing = false;
                    TimeSinceDash = 0;
                }
            }
            else
            {
                Dashing = false;
                TimeSinceDash = 0;
            }
        }
    }
}
