using System;
using UnityEngine;
using Core;
using UnityEngine.InputSystem;

public class Propeller : MonoBehaviour
{
    public enum RotDirection { CW = -1, CCW = 1 };
    
    Rigidbody rb;
    Drone drone;

    public float angular_speed;

    [Header("Technical variables and relations")]
    [SerializeField] public float curr_force;
    [SerializeField] public float max_force = 10f;
    [SerializeField] InputAction use_key;

    [Header("Power")]
    [SerializeField] public float power_drain; 
    [SerializeField] public bool is_power_supplied = true; 

    [Header("Physical characteristics\n(default is DJI MAVIC 3 9453F low-noise) and rough estimations")]
    [SerializeField] public RotDirection rotation_direction = RotDirection.CCW;
    [SerializeField] public float RPM;
    [SerializeField] public float thrust_coefficient = 0.115f;
    [SerializeField] public float torque_coefficient = 0.0065f;
    [SerializeField] public float radius = 0.1194f;

    void Awake()
    {
        if (transform.parent != null)
        {
            drone = transform.parent.GetComponent<Drone>();
            if (drone != null)
            {
                rb = drone.GetComponent<Rigidbody>();
            }
        }
        else
        {
            Debug.LogError(this + " is parentless");
        }
    }

    void FixedUpdate()
    {
        if (drone == null || drone.weather_station == null || rb == null) return;

        // 1. Calculate and update RPM based on current active force
        float airDensity = drone.weather_station.GetAirDensity();
        double diameterPower4 = Math.Pow(radius * 2f, 4);
        float denominator = thrust_coefficient * airDensity * (float)diameterPower4;

        if (denominator > 0f && curr_force > 0f)
        {
            RPM = 60f * (float)Math.Sqrt(curr_force / denominator);
        }
        else
        {
            RPM = 0f;
        }

        // 2. Update power metrics
        UpdatePowerDrain();

        // 3. Continuous Physics application
        if (is_power_supplied && curr_force > 0f)
        {
            rb.AddForceAtPosition(curr_force * transform.up, transform.position, ForceMode.Force);
            ApplyReactiveTorque();
        }
    }

    void UpdatePowerDrain()
    {
        angular_speed = RPM * (float)Math.PI / 30f;
        power_drain = Mathf.Abs(GetReactiveTorque(curr_force) * angular_speed);
    }

    public void TogglePower(bool state)
    {
        is_power_supplied = state;
    }

    public void SetPropellerForce(float i_curr_force)
    {
        if (!is_power_supplied)
        {
            curr_force = 0f;
            return;
        }

        // Warnings for debugging
        if (i_curr_force < 0) Debug.LogWarning("Tried to set propeller activation below zero: " + i_curr_force);
        else if (i_curr_force > max_force) Debug.LogWarning("Tried to set propeller activation above max: " + i_curr_force);

        if (float.IsNaN(i_curr_force) || float.IsInfinity(i_curr_force)) Debug.LogError("error in curr_force");

        // Set the value safely; FixedUpdate will apply the physical force
        curr_force = Mathf.Clamp(i_curr_force, 0f, max_force);
    }

    public void SetPropellerForceFromRatio(float ratio)
    {
        SetPropellerForce(max_force * ratio);
    }

    public void ApplyReactiveTorque()
    {
        // Vector3.up or transform.up depends on system setup; using transform.up matching your code
        rb.AddRelativeTorque(GetReactiveTorque(curr_force) * (int)rotation_direction * (-1f) * transform.up);
    }

    public float GetReactiveTorque(float thrust)
    {
        if (thrust_coefficient == 0f) return 0f;
        float torqueMagnitude = thrust * radius * (torque_coefficient / thrust_coefficient);
        return torqueMagnitude;
    }

    public float CalcuateMaxRpm()
    {
        if (drone == null || drone.weather_station == null) return 1000f;

        float airDensity = drone.weather_station.GetAirDensity();
        double diameterPower4 = Math.Pow(radius * 2f, 4);
        float denominator = thrust_coefficient * airDensity * (float)diameterPower4;

        if (denominator > 0f)
        {
            return 60f * (float)Math.Sqrt(max_force / denominator);
        }
        
        return 1000f;
    }
}
