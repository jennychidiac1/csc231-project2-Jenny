using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float chaseRange = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private Color baseColor;
    private bool poppedIn;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        baseColor = sr.color;
    }

    void OnEnable()
    {
        StartCoroutine(PopIn());
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
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
        if (player == null || !poppedIn) return;

        float dx = player.position.x - transform.position.x;
        if (moveSpeed > 0f && Mathf.Abs(dx) < chaseRange && Mathf.Abs(dx) > 0.5f)
        {
            float dir = Mathf.Sign(dx);
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            sr.flipX = dir < 0f;
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(Flash());
        if (health <= 0) Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = baseColor;
    }
}