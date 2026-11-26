using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects Instance;

    [SerializeField] private GameObject landDust;
    [SerializeField] private GameObject pickupSparkle;

    void Awake()
    {
        Instance = this;
    }

    public void LandDust(Vector3 position)
    {
        Spawn(landDust, position);
    }

    public void PickupSparkle(Vector3 position)
    {
        Spawn(pickupSparkle, position);
    }

    void Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;
        GameObject fx = Instantiate(prefab, position, prefab.transform.rotation);
        Destroy(fx, 2f);
    }
}