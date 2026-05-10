using System;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public enum RotDirection{CW = -1, CCW = 1};
    Rigidbody rb;
    public float angular_speed;
    public double radius;
    public float step;

    [Header("Technical variables and relations")]
    [SerializeField] public float curr_force;
    [SerializeField] public float max_force = 5f;
    [SerializeField] KeyCode use_key;
    [SerializeField] Drone drone;

    [Header("Physical characteristics\n(default is DJI MAVIC 3 9453F low-noise) and rough estimations")]
    [SerializeField] public RotDirection rotation_direction = RotDirection.CCW;
    [SerializeField] public float RPM;//NEXT, RPM DOES NOT CHANGE
    [SerializeField] public float thrust_coefficient = 0.0724f;
    [SerializeField] public float torque_coefficient = 0.0724f * 0.8f;//thr_coeff * 0.5-1.0
    [SerializeField] public float radius_inch = 9.4f;
    [SerializeField] public float step_inch = 5.3f;

    void Awake()
    { 
        rb = drone.GetComponent<Rigidbody>();

        //conversion to meters
        radius = radius_inch/39.37f;
        step = step_inch/39.37f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angular_speed = RPM * 2 * (float)Math.PI / 60f;

        //draw the axes of propellers
        Debug.DrawRay(transform.position, transform.right , Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(use_key)) SetPropellerForce(10f);
        
        ApplyReactiveTorque();
    }
    public void SetPropellerForce(float i_curr_force)
    {   
        if(i_curr_force < 0)              Debug.LogWarning("Tried to set propeller activation below zero: " + i_curr_force); 
        else if(i_curr_force > max_force) Debug.LogWarning("Tried to set propeller activation above max: " + i_curr_force);

        if (float.IsNaN(curr_force) || float.IsInfinity(curr_force))                                                    Debug.LogError("errror in curr_force");
        if (float.IsNaN(transform.up.x) || float.IsNaN(transform.up.y) || float.IsNaN(transform.up.z))                  Debug.LogError("errror in transform.up");
        if (float.IsNaN(transform.position.x) ||float.IsNaN(transform.position.y) || float.IsNaN(transform.position.z)) Debug.LogError("errror in position");

        curr_force = Mathf.Clamp(i_curr_force, 0, max_force);
        rb.AddForceAtPosition(curr_force * transform.up, transform.position, ForceMode.Force);
    }
    public void SetPropellerForceFromRatio(float ratio)
    {   
        if(ratio < 0) Debug.LogWarning("Tried to set propeller activation below zero: " + ratio);
        else if(ratio > 1) Debug.LogWarning("Tried to set propeller activation above max: " + ratio);

        ratio = Mathf.Clamp(ratio, 0, 1);
        curr_force = ratio * max_force;
        rb.AddForceAtPosition(curr_force * transform.up, transform.position, ForceMode.Force);
    }

    public void ApplyReactiveTorque()
    {
        double aerodynamic_resistance = torque_coefficient * drone.weather_station.GetAirDensity() * Math.Pow(radius_inch, 5) * Math.PI;
        double force_amount = angular_speed * angular_speed * aerodynamic_resistance;
        rb.AddRelativeTorque((float)force_amount * (int)rotation_direction * transform.up);
    }
}
