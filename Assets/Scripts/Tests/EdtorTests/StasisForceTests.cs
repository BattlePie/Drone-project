using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Core;
public class StasisForceTests
{
    private class MockDrone: Drone
    {
        public static float FindStasisForceMock(int propeller_count, float drone_mass, Vector3 drone_tilt)
        {
            return FindStasisForce(propeller_count, drone_mass, drone_tilt);
        }
        protected override Dictionary<string, float> SetDroneRotation(Vector3 euler_angles)
        {
            throw new System.NotImplementedException();
        }
    }

    [Test]
    [TestCase(0, 2, 0, 0)]
    [TestCase(4, 10, 30, 0)]
    [TestCase(6, 15, 20, 0)]
    [TestCase(6, 15, 45, 0)]
    [TestCase(6, 15, 60, 0)]
    [TestCase(6, 15, 90, 0)]
    [TestCase(4, 20, 45, 45)]
    [TestCase(6, 20, 60, 60)]

   public void StasisForce_CalculatesCorrectly(int propeller_count, float drone_weight, float drone_roll, float drone_pitch)
{
    float target_force;
    
    Vector3 testAngles = new(drone_pitch, 0, drone_roll);

    float cosRoll = Mathf.Cos(drone_roll * Mathf.Deg2Rad);
    float cosPitch = Mathf.Cos(drone_pitch * Mathf.Deg2Rad);
    float verticalEfficiency = cosRoll * cosPitch;

    if (verticalEfficiency <= 0.001f)
    {
        Assert.That(MockDrone.FindStasisForceMock(propeller_count, drone_weight, testAngles), Is.EqualTo(float.PositiveInfinity));
    }
    else
    {
        target_force = drone_weight * (-Physics.gravity.y) / (verticalEfficiency * propeller_count);
        
        Assert.That(MockDrone.FindStasisForceMock(propeller_count, drone_weight, testAngles), 
            Is.EqualTo(target_force).Within(0.02f), 
            $"Drone: {propeller_count} propellers, {drone_weight} weight, at a {drone_roll}deg roll and {drone_pitch}deg pitch");
    }
}


}
