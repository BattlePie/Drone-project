using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public abstract class Drone : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    public float tilt_ratio;

    [SerializeField] GameObject center_of_mass;
    public Dictionary<string, Propeller> propellers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = center_of_mass.transform.localPosition;
    }

    public abstract void ActivateManualSteering();

}