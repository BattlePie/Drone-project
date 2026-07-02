using UnityEngine;
using static System.Math;
public class AmbientConditions:MonoBehaviour
{
    [SerializeField] public int sea_level_pressure = 101325;
    [SerializeField] public int temperature = 20;

    /// <summary>
/// Calculates the current atmospheric pressure in Pascals based on the drone's height.
/// </summary>
/// <param name="altitude">The current height in meters (typically transform.position.y).</param>
/// <returns>Atmospheric pressure in Pascals (Pa).</returns>
public double CalculatePressureAtAltitude(float altitude)
{
    // Standard Constants
    double p0 = 101325.0;       // Sea-level standard atmospheric pressure (Pa)
    float T0 = 288.15f;         // Sea-level standard temperature (215.15K = 15°C)
    float L = 0.0065f;          // Temperature lapse rate (K/m) in the troposphere
    float g = 9.80665f;         // Acceleration due to gravity (m/s^2)
    float M = 0.0289644f;       // Molar mass of Earth's air (kg/mol)
    float R = 8.3144598f;       // Universal gas constant (J/(mol·K))

    // Prevent negative temperature calculations at extreme altitudes
    if (altitude > 11000f) 
    {
        altitude = 11000f; // Limit to troposphere boundary for this specific formula
    }

    // Temperature at the current altitude
    float currentTempK = T0 - (L * altitude);

    // Barometric formula for the troposphere: P = P0 * (1 - (L*h)/T0) ^ (g*M / R*L)
    double exponent = (g * M) / (R * L);
    double pressure = p0 * Pow(1.0 - ((L * altitude) / T0), exponent);

    return pressure;
}
}
