using System;
using UnityEngine;
using Core;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class Propeller : MonoBehaviour
{
    public enum RotDirection{CW = -1, CCW = 1};
    Rigidbody rb;
    Drone drone;
    public float angular_speed;

    [Header("Technical variables and relations")]
    [SerializeField] public float curr_force;
    [SerializeField] public float max_force = 5f;
    [SerializeField] InputAction use_key;

    [Header("Power")]
    [SerializeField] public float power_drain;          // текущее потребление энергии, Вт
    [SerializeField] public bool is_power_supplied = true; // есть ли питание от аккумулятора

    [Header("Physical characteristics\n(default is DJI MAVIC 3 9453F low-noise) and rough estimations")]
    [SerializeField] public RotDirection rotation_direction = RotDirection.CCW;
    [SerializeField] public float RPM;
    [SerializeField] public float thrust_coefficient = 0.115f;
    [SerializeField] public float torque_coefficient = 0.0065f;
    [SerializeField] public float radius = 0.1194f;
    void Awake()
    { 
        if(transform.parent != null)
        {
        drone = transform.parent.GetComponent<Drone>();
        }
        else
        {
            Debug.Log(this + " is parentless");
        }
        rb = drone.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angular_speed = RPM * (float)Math.PI / 30f;

        //draw the axes of propellers
        Debug.DrawRay(transform.position, transform.right , Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        if (Keyboard.current.spaceKey.isPressed || use_key.WasPressedThisFrame()) SetPropellerForce(10f);
        
        ApplyReactiveTorque();

        UpdatePowerDrain();
    }

    // Мощность = крутящий момент * угловая скорость (P = τ * ω)
    // пересчитываем angular_speed после возможного обновления RPM за этот кадр,
    // чтобы power_drain соответствовал актуальному состоянию пропеллера
    void UpdatePowerDrain()
    {
        angular_speed = RPM * (float)Math.PI / 30f;
        power_drain = Mathf.Abs(GetReactiveTorque(curr_force) * angular_speed);
    }

    public void toggle_power(bool state)
    {
        is_power_supplied = state;
    }

    public void SetPropellerForce(float i_curr_force)
    {   
        if (!is_power_supplied)
        {
            Debug.Log("нет электричества");
            return;
        }

        if(i_curr_force < 0)              Debug.LogWarning("Tried to set propeller activation below zero: " + i_curr_force); 
        else if(i_curr_force > max_force) Debug.LogWarning("Tried to set propeller activation above max: " + i_curr_force);

        if (float.IsNaN(i_curr_force) || float.IsInfinity(i_curr_force))                                                Debug.LogError("errror in curr_force");
        if (float.IsNaN(transform.up.x) || float.IsNaN(transform.up.y) || float.IsNaN(transform.up.z))                  Debug.LogError("errror in transform.up");
        if (float.IsNaN(transform.position.x) ||float.IsNaN(transform.position.y) || float.IsNaN(transform.position.z)) Debug.LogError("errror in position");

        curr_force = Math.Clamp(i_curr_force, 0, max_force);

        float airDensity = (float)drone.weather_station.GetAirDensity();
        float denominator = thrust_coefficient * airDensity * (float)Math.Pow(radius * 2, 4);

        if (denominator <= 0f)
        {
            // защита от деления на ноль (например, если датчик воздуха ещё не готов)
            RPM = 0f;
        }
        else
        {
            RPM = 60f * (float)Math.Sqrt(curr_force / denominator);
        }

        rb.AddForceAtPosition(curr_force * transform.up, transform.position, ForceMode.Force);
    }
    public void SetPropellerForceFromRatio(float ratio)
    {   
        if (!is_power_supplied)
        {
            Debug.Log("нет электричества");
            return;
        }

        if(ratio < 0) Debug.LogWarning("Tried to set propeller activation below zero: " + ratio);
        else if(ratio > 1) Debug.LogWarning("Tried to set propeller activation above max: " + ratio);

        ratio = Mathf.Clamp(ratio, 0, 1);
        curr_force = ratio * max_force;
        rb.AddForceAtPosition(curr_force * transform.up, transform.position, ForceMode.Force);
    }

    public void ApplyReactiveTorque()
    {   
        rb.AddRelativeTorque(GetReactiveTorque(curr_force) * (int)rotation_direction * (-1) * transform.up);
    }

 public float GetReactiveTorque(float thrust)
    {
        // Aerodynamic relationship: Torque = Thrust * Radius * (Cq / Ct)
        float torqueMagnitude = thrust * radius * (torque_coefficient / thrust_coefficient);
        
        return torqueMagnitude;
    }
}
