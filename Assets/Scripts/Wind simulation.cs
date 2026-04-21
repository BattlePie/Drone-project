using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Windsimulation : MonoBehaviour
{
    [SerializeField] public GameObject particles;
    [SerializeField] public LayerMask ignore_layer;
    Collider[] colliders;
    BoxCollider wind_zone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wind_zone = GetComponent<BoxCollider>();
        colliders = Physics.OverlapBox(transform.position, wind_zone.size / 2f, transform.rotation);
        
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.name + " is in the wind zone");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay(Collision collision)
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        
    }
}
