using UnityEngine;

public enum IngredientType { Feather, Jewelry, MandrakeRoot, VoodooDoll, ZombieHand }

public class IngredientPickup : MonoBehaviour
{
    [SerializeField] private IngredientType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() == null) return;
        IngredientInventory.Instance.Add(type);
        Destroy(gameObject);
    }
}