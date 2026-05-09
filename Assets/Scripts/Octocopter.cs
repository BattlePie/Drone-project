using UnityEngine;
using System.Collections.Generic;

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
    protected override void ManualSteering()
    {
        {
        if (Input.GetKey(KeyCode.W))
        {
            propellers["F"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["R"].SetPropellerForceFromRatio(0);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["B"].SetPropellerForceFromRatio(1);
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
        }
        if (Input.GetKey(KeyCode.A))
        {
            propellers["F"].SetPropellerForceFromRatio(0);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["R"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["B"].SetPropellerForceFromRatio(0);
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["L"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
        }
        if (Input.GetKey(KeyCode.S))
        {
            propellers["F"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["R"].SetPropellerForceFromRatio(0);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["B"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["FL"].SetPropellerForceFromRatio(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            propellers["F"].SetPropellerForceFromRatio(0);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["R"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["B"].SetPropellerForceFromRatio(0);
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(1);
        }
        }
    }
}
