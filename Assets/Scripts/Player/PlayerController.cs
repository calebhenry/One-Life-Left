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
    private SpriteRenderer Sprite;
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
    private int Energy = 0;


    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
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

            if (positionChange.sqrMagnitude > 0) 
            {
                Animator.SetBool("Walking", true);
            }
            else
            {
                Animator.SetBool("Walking", false);
            }

            // Determine direction and angle to mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 direction = (mousePos - transform.position).normalized;

            if (Input.GetKey(KeyCode.Space) && !Dashing && Energy >= DashEnergyCost)
            {
                Dashing = true;
                DashDirection = direction;
                Energy = 0;
                GameManager.Instance.Energy = 0;
                Animator.SetBool("Dashing", true);

            }

            if (TimeSinceAttack >= AttackCooldown)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                AttackContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Determine nearest vector direction (using dot product thanks Dr. Yee)
            float right = Vector2.Dot(direction, Vector2.right);
            float left = Vector2.Dot(direction, Vector2.left);
            if (right > left && Sprite.flipX)
            {
                Sprite.flipX = false;
            }
            else if (left > right && !Sprite.flipX)
            {
                Sprite.flipX = true;
            }

            // Start attack, dont allow till cooldown over
            if (Input.GetMouseButton(0) && TimeSinceAttack >= AttackCooldown)
            {
                Animator.SetBool("Attacking", true);
                Attack.StartAttack(direction);
                TimeSinceAttack = 0;
            }
            else if (TimeSinceAttack < AttackCooldown)
            {
                TimeSinceAttack += Time.deltaTime;
            }
        }
        else
        {
            audioSource.Play();
            int numCollisons = RigidBody.Cast(DashDirection, ContactFilter, Collisions, Offset + Time.deltaTime *DashSpeed);
            if (numCollisons == 0)
            {
                RigidBody.MovePosition(RigidBody.position + DashDirection * DashSpeed * Time.deltaTime);
                TimeSinceDash += Time.deltaTime;
                if (TimeSinceDash >= DashTime)
                {
                    Dashing = false;
                    TimeSinceDash = 0;
                    Animator.SetBool("Dashing", false);
                }
            }
            else
            {
                Dashing = false;
                TimeSinceDash = 0;
                Animator.SetBool("Dashing", false);
            }
        }
    }

    public void AddEnergy()
    {
        Energy += 1;
        GameManager.Instance.UpdateEnergy(Energy);
    }
}
