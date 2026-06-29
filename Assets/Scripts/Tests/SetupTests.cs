using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Core;


public class VerticalStabilizationTests
{
    readonly float recovery_time = 8f;
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("VStabTesting");
        yield return null;
    }
    [UnityTest]
    public IEnumerator QuadrocopterVertStabilization_WorksStatically()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Quadrocopter", 0f);
    }
    [UnityTest]
    public IEnumerator OctocopterVertStabilization_WorksStatically()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Octocopter", 0f);
    }
    [UnityTest]
    public IEnumerator HexacopterVertStabilization_WorksStatically()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Hexacopter", 0f);
    }
    [UnityTest]
    public IEnumerator QuadrocopterVertStabilization_WorksFalling()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Quadrocopter", 2f);
    }
    [UnityTest]
    public IEnumerator OctocopterVertStabilization_WorksFalling()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Octocopter", 2f);
    }
    [UnityTest]
    public IEnumerator HexacopterVertStabilization_WorksFalling()
    {
        yield return WhileFalling_DroneVertStabilization_Works("Hexacopter", 2f);
    }
    [UnityTest]
    public IEnumerator Quadrocopter_VertStabilization_WorksFlyingUp()
    {
        yield return WhileFlyingUp_DroneVertStabilization_Works("Quadrocopter");
    }
    [UnityTest]
    public IEnumerator Hexacopter_VertStabilization_WorksFlyingUp()
    {
        yield return WhileFlyingUp_DroneVertStabilization_Works("Hexacopter");
    }
    [UnityTest]
    public IEnumerator Octocopter_VertStabilization_WorksFlyingUp()
    {
        yield return WhileFlyingUp_DroneVertStabilization_Works("Octocopter");
    }
    public IEnumerator WhileFalling_DroneVertStabilization_Works(string drone_name, float fall_time)
    {
        Drone drone = GameObject.Find(drone_name).GetComponent<Drone>();
        drone.transform.position += Vector3.up * 40f;
        Rigidbody rb = drone.GetComponent<Rigidbody>();

        // ACT
        yield return new WaitForSeconds(fall_time);
        drone.ToggleVerticalStabilization(true);
        float expected_height = drone.transform.position.y;
        yield return new WaitForSeconds(recovery_time);

        // ASSERT: Drone retains its height and has no vertical speed
        float currentHeight = drone.transform.position.y;
        bool isHeightCorrect = System.Math.Abs(currentHeight - expected_height) <= 0.05f;

        bool isVelocityCorrect = System.Math.Abs(rb.linearVelocity.y) <= 0.5f; 
        
        Assert.IsTrue(isHeightCorrect && isVelocityCorrect, 
        $"Drone Stabilization Failed!\nExpected height: {expected_height}±0.5 (Got: {currentHeight}),\nExpected Velocity.Y: 0±0.05 (Got: {rb.linearVelocity.y})");

    }
    public IEnumerator WhileFlyingUp_DroneVertStabilization_Works(string drone_name)
    {
        Drone drone = GameObject.Find(drone_name).GetComponent<Drone>();
        Rigidbody rb = drone.GetComponent<Rigidbody>();

        // ACT
        drone.ToggleConstantPropActivation(true, 6f);
        yield return new WaitForSeconds(1f);
        drone.ToggleConstantPropActivation(false);
        float expected_height = drone.transform.position.y;
        drone.ToggleVerticalStabilization(true);
        yield return new WaitForSeconds(recovery_time);

        // ASSERT: Drone retains its height and has no vertical speed
        double currentHeight = drone.transform.position.y;
        bool isHeightCorrect = System.Math.Abs(currentHeight - expected_height) <= 0.05f;

        bool isVelocityCorrect = System.Math.Abs(rb.linearVelocity.y) <= 0.05f; 
        
        Assert.IsTrue(isHeightCorrect && isVelocityCorrect, 
        $"Drone Stabilization Failed! Expected height: {expected_height}±0.05 (Got: {currentHeight}), Expected Velocity.Y: 0±0.05 (Got: {rb.linearVelocity.y})");

    }
}