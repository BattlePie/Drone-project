using System;
using UnityEngine;

public class WeatherStation : MonoBehaviour
{
    [SerializeField] public float max_distance = 100f;
    [SerializeField] public AmbientConditions ambient_conditions;

    // Standard sea-level pressure in Pascals (101325 Pa = 1013.25 hPa/mbar)
    private const double SeaLevelPressure = 101325.0; 

    public void Awake()
    {
        if (GameObject.Find("Ambient Conditions"))
        {
            ambient_conditions = GameObject.Find("Ambient Conditions").GetComponent<AmbientConditions>();
        }
        else
        {
            throw new Exception("No ambient conditions found in current scene");
        }
    }

    public float GetAirDensity()
    {
        float r = 1.225f * (float)Math.Pow(Math.E, -0.02896 * 9.81 * GetHeight() / (8.314 * (GetTemperature_C() + 273.15f)));
        return r;
    }

    public float GetTemperature_C()
    {
        return ambient_conditions.temperature;
    }

    /// <summary>
    /// Simulates the barometer sensor reading. 
    /// In a real Unity setup, this value should decrease as the drone goes higher in world space.
    /// </summary>
public double GetPressure()
{
    // Dynamically calculate pressure based on the drone's current Y position
    double dynamicPressure = ambient_conditions.CalculatePressureAtAltitude(transform.position.y);

    return dynamicPressure;
}

    /// <summary>
    /// Calculates estimated height using barometric pressure conversion.
    /// </summary>
    public float GetHeight()
    {
        double currentPressure = GetPressure();

        // Prevent math errors if pressure values drop to or below zero
        if (currentPressure <= 0)
        {
            return float.PositiveInfinity;
        }

        // Standard International Barometric Formula for altitude
        // h = 44330.77 * (1 - (P / P0)^0.190263)
        float height = 44330.77f * (1.0f - (float)Math.Pow(currentPressure / SeaLevelPressure, 0.190263));

        return height;
    }
}
