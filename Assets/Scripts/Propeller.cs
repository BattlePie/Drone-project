using UnityEngine;

public class Propeller : MonoBehaviour
{
 
    public float curr_force;
    [SerializeField] [Tooltip("Максимальная сила подъёма пропеллера")] public float max_force = 5f;
    [SerializeField] KeyCode use_key;
    [SerializeField] GameObject Drone;
    Rigidbody rb;

    void Start()
    {
        rb = Drone.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(use_key))
        {
            SetPropellerForceFromRatio(1);
        }
        else
        {
            curr_force = 0;
        }
    }
    public void SetPropellerForce(float i_curr_force)
    {   
        if(i_curr_force < 0) curr_force = 0;
        else if(i_curr_force > max_force) curr_force = max_force;
        else curr_force = i_curr_force;

        rb.AddForceAtPosition(curr_force * Vector3.up, transform.position, ForceMode.Force);
    }
/// <summary>
/// Sets propeller curr_force to a ratio of its max curr_force, clamped to [0,1]
/// </summary>
    public void SetPropellerForceFromRatio(float ratio)
    {   
        if(ratio < 0) ratio = 0;
        else if(ratio > 1) ratio = 1;
        else curr_force = max_force * ratio;

        rb.AddForceAtPosition(ratio * max_force * transform.up, transform.position, ForceMode.Force);
    }
}
