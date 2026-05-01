using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]

public abstract class Drone : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    protected float tilt_ratio = 0.8f;
    [SerializeField] GameObject center_of_mass;
    [SerializeField] protected Gyroscope gyroscope;
    [SerializeField] protected Altimeter altimeter;
    [SerializeField] float constant_prop_activation;
    protected Dictionary<string, Propeller> propellers;
    protected Rigidbody rb;
    protected float stasis_force = float.PositiveInfinity;
    protected abstract void ManualSteering();

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        rb.centerOfMass = center_of_mass.transform.localPosition;
    }
    protected virtual void FixedUpdate()
    {
        ManualSteering();
        ConstantPropActivation(constant_prop_activation);
    }
    protected float FindStasisForce(int n_propellers, float max_propeller_force, float drone_mass)//, float tilt)
    {
        //float res = drone_mass * Physics.gravity.magnitude / n_propellers / (float)Math.Cos(tilt);
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