using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]

public abstract class Drone : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    protected float tilt_ratio = 0.2f;
    [SerializeField] GameObject center_of_mass;
    [SerializeField] GameObject i_gyroscope;
    [SerializeField] GameObject i_altimeter;
    [SerializeField] float constant_prop_activation;
    protected Dictionary<string, Propeller> propellers;
    protected Gyroscope gyroscope;
    protected Altimeter altimeter;
    protected Rigidbody rb;
    protected float stasis_force = float.PositiveInfinity;
    protected abstract void ManualSteering();

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = center_of_mass.transform.localPosition;
        gyroscope = i_gyroscope.GetComponent<Gyroscope>();
        altimeter = i_altimeter.GetComponent<Altimeter>();
    }
    protected virtual void FixedUpdate()
    {
        ManualSteering();
        ConstantPropActivation(constant_prop_activation);
    }
    protected float FindStasisForce(int n_propellers, float max_propeller_force, float drone_mass)
    {
        float res = drone_mass * Physics.gravity.magnitude / n_propellers;        

        if(res > max_propeller_force) throw new Exception("Нелетающий дрон, сила стазиса больше максимальной!");
        Debug.Log(n_propellers + "-пропеллерный, С.С.= " + res);
        return res;
    }
    
    public void VerticalStabilization(float target_height)
    {
        float offset = target_height - altimeter.GetReading();
        float target_force = stasis_force + offset - rb.linearVelocity.y;
        Debug.Log("target force = " + target_force);
        foreach(Propeller prop in propellers.Values) prop.SetPropellerForce(target_force);
    }

    public void ConstantPropActivation(float value){

         foreach(Propeller prop in propellers.Values) prop.SetPropellerForce(value);
    }
}