using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
public abstract class Drone : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    protected float tilt_ratio = 0.8f;
    [SerializeField] GameObject center_of_mass;
    [SerializeField] public Gyroscope gyroscope;
    [SerializeField] public Barometer barometer;
    [SerializeField] float constant_prop_activation;
    protected Dictionary<string, Propeller> propellers;
    protected Rigidbody rb;
    protected float stasis_force = float.PositiveInfinity;
    protected float target_v_stab_height;
    protected bool vert_stabilization;

    //bool targeted_flight = false;
    //Vector3 flight_target;
    protected abstract void ManualSteering();
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = center_of_mass.transform.localPosition;
    }
    protected virtual void Start()
    {
        stasis_force = FindStasisForce(propellers.Count, propellers["FL"].max_force, rb.mass);//, Mathf.Deg2Rad * Vector3.Angle(Vector3.up, transform.up));
        Debug.Log("prop count: " + propellers.Count + " max_force: " + propellers["FL"].max_force + " mass: " + rb.mass+"\n" + "stasis force: " + stasis_force);
    }
    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma)){ target_v_stab_height = barometer.GetReading(); vert_stabilization = true;}
        if(Input.GetKeyDown(KeyCode.Period)) vert_stabilization = false;

        //if(Input.GetKeyDown(KeyCode.Semicolon)) {flight_target = transform.position;  targeted_flight = true;}
        //if(Input.GetKeyDown(KeyCode.Quote)) targeted_flight = false;
    }
    protected void FixedUpdate()
    {
        ManualSteering();
        ConstantPropActivation(constant_prop_activation);
        if(vert_stabilization)VerticalStabilization(target_v_stab_height);
    }
    protected float FindStasisForce(int n_propellers, float max_propeller_force, float drone_mass)//, float tilt)
    {
        //float res = drone_mass * Physics.gravity.magnitude / n_propellers / (float)Math.Cos(tilt);
        float res = drone_mass * Physics.gravity.magnitude / n_propellers;

        if(res > max_propeller_force) Debug.LogError("Нелетающий дрон, сила стазиса больше максимальной!");
        return res;
    } 
    public void VerticalStabilization(float target_height)
    {
        Debug.Log(name + " target height: " + target_height);
        float height = barometer.GetReading();
        float target_force;
        if (float.IsInfinity(height))
        {
            Debug.LogWarning(barometer + " can't see the ground, lowering the drone");
            target_force = stasis_force * 0.9f;
            foreach(Propeller prop in propellers.Values) prop.SetPropellerForce(target_force);
            return;

        }
        float offset = target_height - height;
        target_force = stasis_force + offset - rb.linearVelocity.y;
        //Debug.Log("target force = " + target_force);
        if(target_force < 0) target_force = 0;
        foreach(Propeller prop in propellers.Values) prop.SetPropellerForce(target_force);
    }
    public void TiltStabilization()
    {
        throw new NotImplementedException();
    }

    public void ConstantPropActivation(float value){

         foreach(Propeller prop in propellers.Values) prop.SetPropellerForce(value);
    }
}