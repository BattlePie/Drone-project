using System.Collections.Generic;
using UnityEngine;

public class Steeringcomponent : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    float tilt_ratio;
    [SerializeField] GameObject FL_propeller;
    [SerializeField] GameObject FR_propeller;
    [SerializeField] GameObject BL_propeller;
    [SerializeField] GameObject BR_propeller;

    Dictionary<string, Liftcomponent> propellers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        propellers = new Dictionary<string, Liftcomponent>
        {
            ["FL"] = FL_propeller.GetComponent<Liftcomponent>(),
            ["FR"] = FR_propeller.GetComponent<Liftcomponent>(),
            ["BL"] = BL_propeller.GetComponent<Liftcomponent>(),
            ["BR"] = BR_propeller.GetComponent<Liftcomponent>()
        };

    }
    // Update is called once per frame
    void Update()
    {
        //Key to propeller activation
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