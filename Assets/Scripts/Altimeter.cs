using UnityEngine;

public class Altimeter : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    public RaycastHit hit;      


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    public float GetReading()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, max_distance))
        {
            Debug.DrawRay(transform.position, hit.point, Color.red);
            return hit.distance;
        }
        return float.PositiveInfinity;
    }
}
