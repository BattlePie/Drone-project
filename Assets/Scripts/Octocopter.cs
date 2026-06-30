using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

using Core;
public class Octocopter : Drone
{
    [SerializeField] public Propeller F_propeller;
    [SerializeField] public Propeller FR_propeller;
    [SerializeField] public Propeller R_propeller;
    [SerializeField] public Propeller BR_propeller;
    [SerializeField] public Propeller B_propeller;
    [SerializeField] public Propeller BL_propeller;
    [SerializeField] public Propeller L_propeller;
    [SerializeField] public Propeller FL_propeller;

    protected override void Start()
    {
                propellers = new Dictionary<string, Propeller>{
            ["F"] =  F_propeller,
            ["FR"] = FR_propeller,
            ["R"] =  R_propeller,
            ["BR"] = BR_propeller,
            ["B"] =  B_propeller,
            ["BL"] = BL_propeller,
            ["L"] =  L_propeller,
            ["FL"] = FL_propeller};

        base.Start(); 
    }
    protected override Dictionary<string,float> SetDroneRotation(Vector3 euler_angles)
    {
        float roll = euler_angles.x;
        float yaw = euler_angles.y;
        float pitch = euler_angles.z;
        float hover_force = FindStasisForce(propellers.Count, rb.mass, gyroscope.GetReading());
        const float DIAG = 0.7071f; // Pre-calculated sin/cos of 45 degrees

        float f_throttle = hover_force + pitch - yaw;                            // M1 (Front)
        float fr_throttle = hover_force - (DIAG * roll) + (DIAG * pitch) + yaw;  // M2 (Front-Right)
        float r_throttle = hover_force - roll - yaw;                             // M3 (Right)
        float br_throttle = hover_force - (DIAG * roll) - (DIAG * pitch) + yaw;  // M4 (Rear-Right)
        float b_throttle = hover_force - pitch - yaw;                            // M5 (Rear)
        float bl_throttle = hover_force + (DIAG * roll) - (DIAG * pitch) + yaw;  // M6 (Rear-Left)
        float l_throttle = hover_force + roll - yaw;                             // M7 (Left)
        float fl_throttle = hover_force + (DIAG * roll) + (DIAG * pitch) + yaw;  // M8 (Front-Left)

        return new (){
        ["FR"] = fr_throttle,
        ["BL"] = bl_throttle,
        ["FL"] = fl_throttle,
        ["BR"] = br_throttle,
        ["L"] = l_throttle,
        ["R"] = r_throttle,
        ["F"] = f_throttle,
        ["B"] = b_throttle
        };      

    }
}
