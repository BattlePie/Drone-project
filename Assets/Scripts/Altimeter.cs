using UnityEngine;

public class Altimeter : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    RaycastHit hit;      
    LayerMask use_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        use_layer = ~LayerMask.NameToLayer("Ignore Raycast");
    }
    public float GetReading()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, max_distance, use_layer))
        {
            Debug.DrawRay(transform.position, hit.point, Color.red);
            return hit.distance;
        }
        return float.PositiveInfinity;
    }
}
