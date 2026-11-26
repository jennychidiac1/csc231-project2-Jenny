using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Launch(int direction)
    {
        rb.linearVelocity = new Vector2(direction * speed, 0f);
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        bool hitGround = ((1 << other.gameObject.layer) & groundLayer) != 0;
        if (hitGround && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}