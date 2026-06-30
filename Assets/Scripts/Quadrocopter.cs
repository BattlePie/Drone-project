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

    protected override Dictionary<string,float> SetDroneRotation(Vector3 euler_angles)
    {
        float roll = euler_angles.x;
        float yaw = euler_angles.y;
        float pitch = euler_angles.z;
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