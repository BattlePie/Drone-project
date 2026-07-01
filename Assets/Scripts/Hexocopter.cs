using UnityEngine;
using System.Collections.Generic;
using Core;
using UnityEngine.InputSystem;
public class Hexocopter : Drone
{
    [SerializeField] public Propeller FL_propeller;
    [SerializeField] public Propeller FR_propeller;
    [SerializeField] public Propeller BL_propeller;
    [SerializeField] public Propeller BR_propeller;
    [SerializeField] public Propeller L_propeller;
    [SerializeField] public Propeller R_propeller;

    protected override void Start()
    {
        propellers = new Dictionary<string, Propeller>{
            ["FL"] = FL_propeller,
            ["FR"] = FR_propeller,
            ["BL"] = BL_propeller,
            ["BR"] = BR_propeller,
            ["L"] = L_propeller,
            ["R"] = R_propeller};

        base.Start(); 
    }
    protected override Dictionary<string,float> SetDroneRotation(Vector3 target_angles)
    {
        float p_gain = 5f;
        float d_gain = 1.5f;
        float curr_roll =  gyroscope.GetReading().x;
        float curr_yaw =   gyroscope.GetReading().y;
        float curr_pitch = gyroscope.GetReading().z;

        // 1. Считаем пропорциональную ошибку (на сколько градусов мы отклонились)
        float error_roll = curr_roll - target_angles.x;
        float error_yaw = curr_yaw - target_angles.y;
        float error_pitch = curr_pitch - target_angles.z;

        // 2. Берем текущую угловую скорость из Rigidbody (оси могут зависеть от ориентации дрона)
        // Если rb.angularVelocity в локальных координатах, используйте rb.transform.InverseTransformDirection(rb.angularVelocity)
        Vector3 local_angular_vel = transform.InverseTransformDirection(rb.angularVelocity);

        // 3. PD-регулятор: (Ошибка * P) - (Угловая скорость * D)
        float roll = (error_roll * p_gain) - (local_angular_vel.x * d_gain);
        float yaw = (error_yaw * p_gain) - (local_angular_vel.y * d_gain);
        float pitch = (error_pitch * p_gain) - (local_angular_vel.z * d_gain);


        float hover_force = FindStasisForce(propellers.Count, rb.mass, gyroscope.GetReading());

        float fr_throttle = hover_force - (0.500f * roll) + (0.866f * pitch) + yaw; // Front-Right (CCW)
        float bl_throttle = hover_force + (0.500f * roll) - (0.866f * pitch) + yaw; // Rear-Left (CCW)
        float l_throttle  = hover_force + (1.000f * roll) - yaw;                    // Middle-Left (CW)
        float fl_throttle = hover_force + (0.500f * roll) + (0.866f * pitch) - yaw; // Front-Left (CW)
        float br_throttle = hover_force - (0.500f * roll) - (0.866f * pitch) - yaw; // Rear-Right (CW)
        float r_throttle  = hover_force - (1.000f * roll) + yaw;                    // Middle-Right (CCW)
        
        return new (){
        ["FR"] = fr_throttle,
        ["BL"] = bl_throttle,
        ["FL"] = fl_throttle,
        ["BR"] = br_throttle,
        ["L"] = l_throttle,
        ["R"] = r_throttle
        };      

    }    
}
