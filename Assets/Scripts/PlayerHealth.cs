using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Image[] hearts;
    [SerializeField] private float invincibleTime = 1f;
    [SerializeField] private float respawnDelay = 1.5f;

    private int currentHealth;
    private bool isDead;
    private float invincibleUntil;
    private Vector3 respawnPosition;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerController controller;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        respawnPosition = transform.position;
    }

    void Start()
    {
        UpdateHearts();
    }

    public void SetRespawn(Vector3 pos)
    {
        respawnPosition = pos;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || Time.time < invincibleUntil) return;

        currentHealth -= amount;
        UpdateHearts();

        if (currentHealth <= 0)
        {
            StartCoroutine(DieRoutine());
        }
        else
        {
            animator.SetTrigger("Hurt");
            invincibleUntil = Time.time + invincibleTime;
        }
    }

    public void FallRespawn()
    {
        if (isDead) return;
        rb.linearVelocity = Vector2.zero;
        transform.position = respawnPosition;
        invincibleUntil = 0f;
        TakeDamage(1);
    }

    IEnumerator DieRoutine()
    {
        isDead = true;
        controller.canMove = false;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsGrounded", true);
        animator.SetFloat("YVelocity", 0f);
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    void Respawn()
    {
        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;
        currentHealth = maxHealth;
        UpdateHearts();
        isDead = false;
        controller.canMove = true;
        invincibleUntil = Time.time + invincibleTime;
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Dead");
        animator.Play("Idle");
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}