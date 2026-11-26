using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 5f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private bool spriteFacesRight = true;
    [SerializeField] private float deathTime = 1.2f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float hitDelay = 0.3f;
    [SerializeField] private int attackDamage = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private Transform player;
    private PlayerHealth playerHealth;
    private Color baseColor;
    private bool poppedIn;
    private bool dead;
    private bool attacking;
    private bool useSecondAttack;
    private float nextAttackTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        baseColor = sr.color;
    }

    void OnEnable()
    {
        StartCoroutine(PopIn());
    }

    void Start()
    {
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        player = pc.transform;
        playerHealth = pc.GetComponent<PlayerHealth>();
    }

    IEnumerator PopIn()
    {
        poppedIn = false;
        Vector3 full = transform.localScale;
        float t = 0f;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(full * 0.1f, full, t / 0.25f);
            yield return null;
        }
        transform.localScale = full;
        poppedIn = true;
    }

    void FixedUpdate()
    {
        if (dead || player == null || !poppedIn) return;

        if (attacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float dx = player.position.x - transform.position.x;
        float dist = Mathf.Abs(dx);
        float dir = Mathf.Sign(dx);

        if (dist < chaseRange)
        {
            sr.flipX = spriteFacesRight ? dir < 0f : dir > 0f;
        }

        bool inAttackRange = dist <= attackRange
            && Mathf.Abs(player.position.y - transform.position.y) < 1.5f;

        if (inAttackRange)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            SetSpeed(0f);
            if (Time.time >= nextAttackTime) StartCoroutine(Attack());
        }
        else if (moveSpeed > 0f && dist < chaseRange)
        {
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            SetSpeed(1f);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            SetSpeed(0f);
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        nextAttackTime = Time.time + attackCooldown;
        if (animator != null) animator.SetTrigger(useSecondAttack ? "Attack2" : "Attack1");
        useSecondAttack = !useSecondAttack;

        yield return new WaitForSeconds(hitDelay);

        if (!dead && playerHealth != null)
        {
            float dist = Vector2.Distance(player.position, transform.position);
            if (dist <= attackRange + 0.3f) playerHealth.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }

    void SetSpeed(float value)
    {
        if (animator != null) animator.SetFloat("Speed", value);
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;
        health -= amount;

        if (health <= 0f) StartCoroutine(Die());
        else StartCoroutine(Flash());
    }

    IEnumerator Die()
    {
        dead = true;
        rb.linearVelocity = Vector2.zero;
        foreach (Collider2D c in GetComponents<Collider2D>()) c.enabled = false;
        rb.simulated = false;
        if (animator != null) animator.SetTrigger("Dead");
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = baseColor;
    }
}