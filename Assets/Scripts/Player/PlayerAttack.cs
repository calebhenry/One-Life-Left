using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttackLength = 0.3f;
    public int AttackDamage = 1;
    public GameObject AttackModel;
    private float LastAttack = 0;
    private Collider2D Collider;
    private Animator Animator;
    private PlayerController PlayerController;
    private Vector2 AttackDirection;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<Collider2D>();
        Animator = GetComponentInParent<Animator>();
        PlayerController = GetComponentInParent<PlayerController>();
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
            Collider.enabled = false;
            AttackModel.SetActive(false);
            Animator.SetBool("Attacking", false);
        }
    }

    public void StartAttack(Vector2 direction)
    {
        Collider.enabled = true;
        LastAttack = AttackLength;
        AttackModel.SetActive(true);
        AttackDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy": case "Boss":
                if (collision.GetType() != typeof(CircleCollider2D))
                {
                    NPCHealth health = collision.GetComponent<NPCHealth>();
                    health.TakeDamage(1);
                    PlayerController.AddEnergy();
                }
                break;
            case "Projectile":
                ProjectileManager projectile = collision.GetComponent<ProjectileManager>();
                projectile.Deflect(AttackDirection);
                PlayerController.AddEnergy();
                break;
            case "Breakable":
                Breakable breakable = collision.GetComponent<Breakable>();
                breakable.Break();
                break;
        }
    }
}
