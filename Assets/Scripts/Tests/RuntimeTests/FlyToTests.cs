using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Core;
using System;

public class FlyToTests : MonoBehaviour
{
    private readonly static string[] DroneTypes = new string[] { "Quadrocopter", "Octocopter", "Hexacopter" };

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Testing Scene");
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
        return MakeTargets(Mathf.PI/4f, 10f, Vector3.up * 3);
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
    public IEnumerator FlyTowardsTest(string drone_prefab_path, Vector3 target)
    {
        // ARRANGE
        GameObject drone_instance = Instantiate(Resources.Load<GameObject>(drone_prefab_path), new Vector3(0,3,0), Quaternion.identity);
        Drone drone = drone_instance.GetComponent<Drone>();
        Rigidbody rb = drone.GetComponent<Rigidbody>();

        
        throw new NotImplementedException();
    }
    public IEnumerator FlyTowardsAndStopAtTest(string drone_prefab_path, Vector3 target)
    {
        // ARRANGE
        throw new NotImplementedException();
    }
}