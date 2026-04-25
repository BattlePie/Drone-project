using UnityEngine;
using System.Collections.Generic;

public class Octocopter : Drone
{
    [SerializeField] GameObject F_propeller;
    [SerializeField] GameObject FR_propeller;
    [SerializeField] GameObject R_propeller;
    [SerializeField] GameObject BR_propeller;
    [SerializeField] GameObject B_propeller;
    [SerializeField] GameObject BL_propeller;
    [SerializeField] GameObject L_propeller;
    [SerializeField] GameObject FL_propeller;
    bool vert_stabilization = false;
    bool targeted_flight = false;
    Vector3 flight_target;

    protected override void Start()
    {base.Start();
        propellers = new Dictionary<string, Propeller>
        {
            ["F"] = F_propeller.GetComponent<Propeller>(),
            ["FR"] = FR_propeller.GetComponent<Propeller>(),
            ["R"] = R_propeller.GetComponent<Propeller>(),
            ["BR"] = BR_propeller.GetComponent<Propeller>(),
            ["B"] = B_propeller.GetComponent<Propeller>(),
            ["BL"] = BL_propeller.GetComponent<Propeller>(),
            ["L"] = L_propeller.GetComponent<Propeller>(),
            ["FL"] = FL_propeller.GetComponent<Propeller>()
        };
        stasis_force = FindStasisForce(propellers.Count, propellers["FL"].max_force, rb.mass);  
        //stasis_force = 1.23f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma))  vert_stabilization = true;
        if(Input.GetKeyDown(KeyCode.Period)) vert_stabilization = false;

        if(Input.GetKeyDown(KeyCode.Semicolon)) {flight_target = transform.position;  targeted_flight = true;}
        if(Input.GetKeyDown(KeyCode.Quote)) targeted_flight = false;
    }
    protected override void FixedUpdate()
    {base.FixedUpdate();

        if(vert_stabilization)VerticalStabilization(altimeter.GetReading());

        //Fly to target
        if (targeted_flight)
        {}
        
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
