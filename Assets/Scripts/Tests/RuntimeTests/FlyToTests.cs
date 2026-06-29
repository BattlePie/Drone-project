using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Core;
using System;

public class FlyToTests
{
    private readonly static string[] DroneTypes = new string[] { "Quadrocopter", "Octocopter", "Hexacopter" };
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("VStabTesting");
        yield return null;
    }
    [UnityTest]
    public IEnumerator Drone_Flies_To_Target([ValueSource(nameof(DroneTypes))] string drone_name, [ValueSource(nameof(SetTargets))] Vector3 target)
    {
        yield return FlyTowardsTest(drone_name, target);
    }
    [UnityTest]
    public IEnumerator Drone_Stops_On_Target([ValueSource(nameof(DroneTypes))] string drone_name, [ValueSource(nameof(SetTargets))] Vector3 target)
    {
        yield return FlyTowardsAndStopAtTest(drone_name, target);
    }
    public static IEnumerable<Vector3> SetTargets()
    {   
        return MakeTargets(Mathf.PI/6f, 10f, Vector3.up * 3);
    }
    public static IEnumerable<Vector3> MakeTargets(float angle_step, float distance, Vector3 center)
    {
        float curr_angle = angle_step;

        while(curr_angle <= Mathf.PI * 2)
        {
        yield return new(Mathf.Cos(curr_angle) * distance + center.x, center.y, Mathf.Sin(curr_angle) * distance + center.z);
        curr_angle += angle_step;
        }
    }

    public IEnumerator FlyTowardsTest(string drone_name, Vector3 target)
    {
        // ARRANGE
        throw new NotImplementedException();
    }

    public IEnumerator FlyTowardsAndStopAtTest(string drone_name, Vector3 target)
    {
        // ARRANGE
        throw new NotImplementedException();
    }
}