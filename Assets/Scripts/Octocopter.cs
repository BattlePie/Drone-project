using UnityEngine;
using System.Collections.Generic;

public class Octocopter : Drone
{
    [SerializeField] Propeller F_propeller;
    [SerializeField] Propeller FR_propeller;
    [SerializeField] Propeller R_propeller;
    [SerializeField] Propeller BR_propeller;
    [SerializeField] Propeller B_propeller;
    [SerializeField] Propeller BL_propeller;
    [SerializeField] Propeller L_propeller;
    [SerializeField] Propeller FL_propeller;
    bool vert_stabilization = false;
    bool targeted_flight = false;
    Vector3 flight_target;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {base.Start();
        propellers = new Dictionary<string, Propeller>
        {
            ["F"] =  F_propeller,
            ["FR"] = FR_propeller,
            ["R"] =  R_propeller,
            ["BR"] = BR_propeller,
            ["B"] =  B_propeller,
            ["BL"] = BL_propeller,
            ["L"] =  L_propeller,
            ["FL"] = FL_propeller
        };
        stasis_force = FindStasisForce(propellers.Count, propellers["FL"].max_force, rb.mass);//, Mathf.Deg2Rad * Vector3.Angle(Vector3.up, transform.up));
        // max propeller force should be the smallest of all of propellers
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
