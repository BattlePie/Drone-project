using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

using Core;
public class Quadrocopter : Drone
{
    public Propeller FL_propeller;
    public Propeller FR_propeller;
    public Propeller BL_propeller;
    public Propeller BR_propeller;
    protected override void Start()
    {
        propellers = new Dictionary<string, Propeller> {
            ["FL"] = FL_propeller,
            ["FR"] = FR_propeller,
            ["BL"] = BL_propeller,
            ["BR"] = BR_propeller};        

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

        float fr_throttle = hover_force - roll + pitch + yaw;
        float bl_throttle = hover_force + roll - pitch + yaw;   
        float fl_throttle = hover_force + roll + pitch - yaw;
        float br_throttle = hover_force - roll - pitch - yaw;
        
        return new (){
        ["FR"] = fr_throttle,
        ["BL"] = bl_throttle,
        ["FL"] = fl_throttle,
        ["BR"] = br_throttle
        };      

    }
}