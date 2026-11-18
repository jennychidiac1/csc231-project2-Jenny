using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Vector2 respawnOffset = new Vector2(0f, 1f);
    [SerializeField] private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = inactiveColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;
        health.SetRespawn(transform.position + (Vector3)respawnOffset);
        if (sr != null) sr.color = Color.white;
    }
}