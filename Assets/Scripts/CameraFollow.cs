using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);
    [SerializeField] private float minX = -100f;
    [SerializeField] private float maxX = 100f;

    void LateUpdate()
    {
        Vector3 desired = target.position + offset;
        desired.x = Mathf.Clamp(desired.x, minX, maxX);
        transform.position = Vector3.Lerp(
            transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}