using UnityEngine;
using System.Collections.Generic;

public class Hexocopter : Drone
{
    [SerializeField] Propeller FL_propeller;
    [SerializeField] Propeller FR_propeller;
    [SerializeField] Propeller BL_propeller;
    [SerializeField] Propeller BR_propeller;
    [SerializeField] Propeller L_propeller;
    [SerializeField] Propeller R_propeller;
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
            ["FL"] = FL_propeller,
            ["FR"] = FR_propeller,
            ["BL"] = BL_propeller,
            ["BR"] = BR_propeller,
            ["L"] = L_propeller,
            ["R"] = R_propeller
        };
        stasis_force = FindStasisForce(propellers.Count, propellers["FL"].max_force, rb.mass);//,Mathf.Deg2Rad * Vector3.Angle(Vector3.up, transform.up));  
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
    override protected void ManualSteering()
    {
        {
        if (Input.GetKey(KeyCode.W))
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["R"].SetPropellerForceFromRatio(0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["R"].SetPropellerForceFromRatio(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);
            propellers["L"].SetPropellerForceFromRatio(0);
            propellers["R"].SetPropellerForceFromRatio(0);
        }
        if (Input.GetKey(KeyCode.D))
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
