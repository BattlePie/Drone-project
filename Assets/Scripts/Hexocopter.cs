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
    protected override void Controller(Vector3 euler_angles, float throttle)
    {
        
    }
    override protected void ManualSteering()
    {
        {
        if (Keyboard.current.wKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["R"].SetPropellerForceFromRatio(0);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["R"].SetPropellerForceFromRatio(1);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["R"].SetPropellerForceFromRatio(0);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["L"].SetPropellerForceFromRatio(1);
            propellers["R"].SetPropellerForceFromRatio(tilt_ratio);
        }
        }
    }

    
}
