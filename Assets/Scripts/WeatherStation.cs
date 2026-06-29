using System;
using UnityEngine;

public class WeatherStation : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    [SerializeField] public AmbientConditions ambient_conditions;
    public void Awake()
    {
        if(GameObject.Find("Ambient Conditions"))
        {
            ambient_conditions = GameObject.Find("Ambient Conditions").GetComponent<AmbientConditions>();
        }
        else 
        {
            throw new Exception("No ambient conditions found in current scene");
        }
    }
    public double GetAirDensity()
    {   double r = 1.225f * Math.Pow(Math.E, -0.02896 * 9.81 * GetHeight()/(8.314 * (GetTemperature_C() + 273.15f)));// M_air*g*h/(R*T)
        //Debug.Log("Air density:" + r);
        return r;
    }
    public double GetTemperature_C()
    {
        return ambient_conditions.temperature;
    }
    public double GetPressure()
    {
        return ambient_conditions.atmospheric_pressure;
    }

    public float GetHeight()/// TO BE UPDATED, USES ALTIMETER LOGIC
    {
        bool hitAnything = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, max_distance);
        Debug.DrawRay(transform.position, Vector3.down * max_distance, hitAnything ? Color.green : Color.red);

        if (hitAnything)
        {
            //Debug.Log("Altimeter sees: " + hitInfo.collider.name);
            return hitInfo.distance;
        }

        return float.PositiveInfinity;
    }
}
