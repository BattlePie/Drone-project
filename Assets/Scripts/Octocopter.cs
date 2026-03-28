using UnityEngine;
using System.Collections.Generic;

public class Octocopter : Drone
{
    [SerializeField] GameObject FL_propeller;
    [SerializeField] GameObject FR_propeller;
    [SerializeField] GameObject LF_propeller;
    [SerializeField] GameObject RF_propeller;
    [SerializeField] GameObject LB_propeller;
    [SerializeField] GameObject RB_propeller;
    [SerializeField] GameObject BL_propeller;
    [SerializeField] GameObject BR_propeller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        propellers = new Dictionary<string, Propeller>
        {
            ["FL"] = FL_propeller.GetComponent<Propeller>(),
            ["FR"] = FR_propeller.GetComponent<Propeller>(),
            ["LF"] = LF_propeller.GetComponent<Propeller>(),
            ["RF"] = RF_propeller.GetComponent<Propeller>(),
            ["LB"] = LB_propeller.GetComponent<Propeller>(),
            ["RB"] = RB_propeller.GetComponent<Propeller>()
            ["BL"] = BL_propeller.GetComponent<Propeller>(),
            ["BR"] = BR_propeller.GetComponent<Propeller>()
        };
    }

    // Update is called once per frame
    void Update()
    {
        ActivateManualSteering();
    }

    override public void ActivateManualSteering()
    {
        {
        if (Input.GetKey(KeyCode.W))
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["LF"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["RF"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["LB"].SetPropellerForceFromRatio(1);
            propellers["RB"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
        }
        if (Input.GetKey(KeyCode.A))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(1);
            propellers["LF"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["RF"].SetPropellerForceFromRatio(1);
            propellers["LB"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["RB"].SetPropellerForceFromRatio(1);
            propellers["FL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FR"].SetPropellerForceFromRatio(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            propellers["BL"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["LF"].SetPropellerForceFromRatio(1);
            propellers["RF"].SetPropellerForceFromRatio(1);
            propellers["LB"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["RB"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            propellers["BL"].SetPropellerForceFromRatio(1);
            propellers["BR"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["LF"].SetPropellerForceFromRatio(1);
            propellers["RF"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["LB"].SetPropellerForceFromRatio(1);
            propellers["RB"].SetPropellerForceFromRatio(tilt_ratio);
            propellers["FL"].SetPropellerForceFromRatio(1);
            propellers["FR"].SetPropellerForceFromRatio(tilt_ratio);
        }
        }
    }
}
