using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Quadrocopter : Drone
{
//~2.45 all prop activation for stasis

    [SerializeField] GameObject FL_propeller;
    [SerializeField] GameObject FR_propeller;
    [SerializeField] GameObject BL_propeller;
    [SerializeField] GameObject BR_propeller;
    bool activate_stabilization = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {base.Start();
        propellers = new Dictionary<string, Propeller> {
            ["FL"] = FL_propeller.GetComponent<Propeller>(),
            ["FR"] = FR_propeller.GetComponent<Propeller>(),
            ["BL"] = BL_propeller.GetComponent<Propeller>(),
            ["BR"] = BR_propeller.GetComponent<Propeller>()};        

        //stasis_force = FindStasisForce(propellers.Count, propellers[0].max_force, rb.mass);  
        stasis_force = 2.45f;
    }

    protected override void Update()
    {base.Update();

        if(Input.GetKeyDown(KeyCode.Comma))  activate_stabilization = true;
        if(Input.GetKeyDown(KeyCode.Period)) activate_stabilization = false;
        
        if(activate_stabilization)VerticalStabilization(altimeter.GetReading());
    }

override public void ManualSteering()
    {
        {
        if (Input.GetKey(KeyCode.W))
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);

        }
        if (Input.GetKey(KeyCode.A))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);

        }
        if (Input.GetKey(KeyCode.D))
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
        }
        }
    }
}