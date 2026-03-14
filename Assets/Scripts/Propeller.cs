using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] [Tooltip("Максимальная сила подъёма пропеллера")] public float max_lift_force;
    [SerializeField] [Tooltip("К какому дрону прикреплён пропеллер")]GameObject main_body;
    [SerializeField] KeyCode use_key;
    Rigidbody rb;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = main_body.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(use_key))
        {
            SetPropellerForceFromRatio(1);
        }
    }
    public void SetPropellerForce(float force)
    {
        if(force < 0) force = 0;
        if(force > max_lift_force) force = max_lift_force;

        rb.AddForce(force * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
    }
/// <summary>
/// Sets propeller force to a ratio of its max force, clamped to [0,1]
/// </summary>
    public void SetPropellerForceFromRatio(float ratio)
    {
        if(ratio < 0) ratio = 0;
        if(ratio > 1) ratio = 1;

        rb.AddForceAtPosition(ratio * max_lift_force * Time.deltaTime * transform.up, transform.position, ForceMode.VelocityChange);
    }
}
