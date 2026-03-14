using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class Steeringcomponent : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    float tilt_ratio;
    [SerializeField] GameObject FL_propeller;
    [SerializeField] GameObject FR_propeller;
    [SerializeField] GameObject BL_propeller;
    [SerializeField] GameObject BR_propeller;
    [SerializeField] GameObject center_of_mass;
    Dictionary<string, Propeller> propellers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        propellers = new Dictionary<string, Propeller>
        {
            ["FL"] = FL_propeller.GetComponent<Propeller>(),
            ["FR"] = FR_propeller.GetComponent<Propeller>(),
            ["BL"] = BL_propeller.GetComponent<Propeller>(),
            ["BR"] = BR_propeller.GetComponent<Propeller>()
        };
        GetComponent<Rigidbody>().centerOfMass = center_of_mass.transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        ActivateManualSteering();
    }

    void ActivateManualSteering()
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