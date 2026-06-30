using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

using Core;
public class Quadrocopter : Drone
{
    [SerializeField] public Propeller FL_propeller;
    [SerializeField] public Propeller FR_propeller;
    [SerializeField] public Propeller BL_propeller;
    [SerializeField] public Propeller BR_propeller;

    protected override void Start()
    {
        propellers = new Dictionary<string, Propeller> {
            ["FL"] = FL_propeller,
            ["FR"] = FR_propeller,
            ["BL"] = BL_propeller,
            ["BR"] = BR_propeller};        

        base.Start();       
    }

    protected override void Controller(UnityEngine.Vector3 euler_angles, float throttle)
    {
        float fl_throttle = throttle - euler_angles.x + euler_angles.y + euler_angles.z;

        propellers["FL"].SetPropellerForce(throttle - euler_angles.x + euler_angles.y + euler_angles.z);


        //Assumes regular quadrocopter propeller positions
        

    }

    protected override void ManualSteering()
    {
        {
        if (Keyboard.current.wKey.isPressed)
        {

            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);

        }
        if (Keyboard.current.aKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(1);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);

        }
        if (Keyboard.current.dKey.isPressed)
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
        }
        }
    }
}