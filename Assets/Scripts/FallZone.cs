using UnityEngine;

public class FallZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("FallZone touched by: " + other.name);
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null) health.FallRespawn();
    }
}