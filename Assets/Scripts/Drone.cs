using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]

public abstract class Drone : MonoBehaviour
{
    [SerializeField] [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
    public float tilt_ratio = 0.2f;
    [SerializeField] public float stabilization_force = 20f;
    [SerializeField] public GameObject center_of_mass;
    [SerializeField] public GameObject i_gyroscope;
    [SerializeField] public GameObject i_altimeter;
    protected Dictionary<string, Propeller> propellers;
    protected Gyroscope gyroscope;
    protected Altimeter altimeter;
    protected Rigidbody rb;
    protected float stasis_force = float.PositiveInfinity;
    public abstract void ManualSteering();

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = center_of_mass.transform.localPosition;
        gyroscope = i_gyroscope.GetComponent<Gyroscope>();
        altimeter = i_altimeter.GetComponent<Altimeter>();

        //Debug.Assert(FindStasisForce(4, 10, 1) > 2f && FindStasisForce(4, 10, 1) < 3f, 
        //"Дрон с параметрами (4 проп, max_force = 10, weight = 1) имеет силу стазиса 2<F<3, получилось" + FindStasisForce(4, 10, 1));
    }

    protected virtual void Update()
    {
        ManualSteering();
    }
    public float FindStasisForce(int n_propellers, float max_propeller_force, float drone_weight)
    {
        float res = 0;

        throw new NotImplementedException();

        if(max_propeller_force > res) throw new Exception("Нелетающий дрон, сила стазиса больше максимальной!");
        return res;
    }
    
    public void VerticalStabilization(float target_height)
    {
        float offset = target_height - altimeter.GetReading();
        Debug.Log("offset from target: " + offset);
        float target_force = stasis_force + stabilization_force * offset - rb.linearVelocity.y;
        Debug.Log("Prop force changed to: " + target_force);
        foreach(Propeller prop in propellers.Values)
        {
            prop.SetPropellerForce(target_force);
        }
    }
}