using UnityEngine;

public class Propeller : MonoBehaviour
{
    public enum RotDirection{CW = -1, CCW = 1};
    Rigidbody rb;
    [SerializeField] public float curr_force;
    [SerializeField] [Tooltip("Максимальная сила подъёма пропеллера")] public float max_force = 5f;
    [SerializeField] KeyCode use_key;
    [SerializeField] Drone Drone;
    [SerializeField] public RotDirection rotation_direction = RotDirection.CCW;
    void Start()
    { 
        rb = Drone.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(use_key)) SetPropellerForceFromRatio(1);
        else curr_force = 0;
        ApplyReactiveTorque();
    }
    public void SetPropellerForce(float i_curr_force)
    {   
        if(i_curr_force < 0) Debug.LogWarning("Tried to set propeller activation below zero: " + i_curr_force); 
        else if(i_curr_force > max_force) Debug.LogWarning("Tried to set propeller activation above max: " + i_curr_force);

        curr_force = Mathf.Clamp(i_curr_force, 0, max_force);
        rb.AddForceAtPosition(curr_force * Vector3.up, transform.position, ForceMode.Force);
    }
/// <summary>
/// Sets propeller curr_force to a ratio of its max curr_force, clamped to [0,1]
/// </summary>
    public void SetPropellerForceFromRatio(float ratio)
    {   
        if(ratio < 0) Debug.LogWarning("Tried to set propeller activation below zero: " + ratio);
        else if(ratio > 1) Debug.LogWarning("Tried to set propeller activation above max: " + ratio);

        curr_force = Mathf.Clamp(ratio, 0, 1);
        rb.AddForceAtPosition(ratio * max_force * transform.up, transform.position, ForceMode.Force);
    }

    public void ApplyReactiveTorque()
    {
        float force_amount = 0;///FIND AND CHANGE TO THE FORMULA OF REACTIVE TORQUE
        rb.AddRelativeTorque(Vector3.up * force_amount * (int)rotation_direction);
    }
}
