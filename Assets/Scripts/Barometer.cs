using UnityEngine;

public class Barometer : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    RaycastHit hit;
    LayerMask use_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        use_layer = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
    }

    public float GetReading()
    {
        bool hitAnything = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, max_distance);
        Debug.DrawRay(transform.position, Vector3.down * max_distance, hitAnything ? Color.green : Color.red);

        if (hitAnything)
        {
            Debug.Log("Altimeter sees: " + hitInfo.collider.name);
            return hitInfo.distance;
        }

        return float.PositiveInfinity;
    }
}
