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
    protected override Dictionary<string,float> SetDroneRotation(Vector3 euler_angles)
    {
        float roll = euler_angles.x;
        float yaw = euler_angles.y;
        float pitch = euler_angles.z;
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
