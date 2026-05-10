using System;
using UnityEngine;

public class WeatherStation : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    [SerializeField] public Ambientconditions ambientconditions;
    RaycastHit hit;
    LayerMask use_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        use_layer = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
    }
    
    public double GetAirDensity()
    {   double r = 1.225f * Math.Pow(Math.E, -0.02896 * 9.81 * GetHeight()/(8.314 * (GetTemperature_C() + 273.15f)));// M_air*g*h/(R*T)
        Debug.Log("Air density:" + r);
        return r;
    }
    public double GetTemperature_C()
    {
        return ambientconditions.temperature;
    }
    public double GetPressure()
    {
        return ambientconditions.atmospheric_pressure;
    }

    public float GetHeight()/// TO BE UPDATED, USES ALTIMETRY LOGIC
    {
        bool hitAnything = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, max_distance);
        Debug.DrawRay(transform.position, Vector3.down * max_distance, hitAnything ? Color.green : Color.red);

        if (hitAnything)
        {
            Debug.Log("Altimeter sees: " + hitInfo.collider.name);
            return hitInfo.distance;
        }

        return float.PositiveInfinity;
    }
}
