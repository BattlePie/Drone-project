using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Core;


public class VerticalStabilizationTests : MonoBehaviour
{

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Testing Scene");
        yield return null;
    }

    private readonly static string[] DronePaths = { "Prefabs/Quadrocopter", "Prefabs/Octocopter", "Prefabs/Hexacopter" };
    readonly float recovery_time = 8f;
    [UnityTest]
    public IEnumerator VertStabilization_WorksStatically([ValueSource(nameof(DronePaths))] string drone_prefab_path)
    {
        yield return WhileFalling_DroneVertStabilization_Works(drone_prefab_path, 0f);
    }

    [UnityTest]
    public IEnumerator VertStabilization_WorksFalling([ValueSource(nameof(DronePaths))] string drone_prefab_path)
    {
        yield return WhileFalling_DroneVertStabilization_Works(drone_prefab_path, 2f);
    }
    [UnityTest]
    public IEnumerator VertStabilization_WorksFlyingUp([ValueSource(nameof(DronePaths))] string drone_prefab_path)
    {
        yield return WhileFlyingUp_DroneVertStabilization_Works(drone_prefab_path);
    }
    public IEnumerator WhileFalling_DroneVertStabilization_Works(string drone_prefab_path, float fall_time)
    {
        // ARRANGE
        GameObject drone_instance = Instantiate(Resources.Load<GameObject>(drone_prefab_path), new Vector3(0,3,0), Quaternion.identity);
        Drone drone = drone_instance.GetComponent<Drone>();
        Rigidbody rb = drone.GetComponent<Rigidbody>();
        drone.transform.position = Vector3.up * 40f;

        // ACT
        float expected_height = drone.transform.position.y;
        if (fall_time > 0)
        {yield return new WaitForSeconds(fall_time);
        expected_height = drone.transform.position.y;}

        drone.ToggleVerticalStabilization(true, expected_height);
        yield return new WaitForSeconds(recovery_time);

        // ASSERT: Drone retains its height and has no vertical speed
float current_height = drone.transform.position.y;
        bool isHeightCorrect = System.Math.Abs(current_height - expected_height) <= 0.5f;

        bool isVelocityCorrect = System.Math.Abs(rb.linearVelocity.y) <= 0.5f; 
        
        Assert.IsTrue(isHeightCorrect && isVelocityCorrect, 
         $"Drone Stabilization Failed!\nExpected height: {expected_height}±0.5 (Got: {current_height} Delta: {expected_height - current_height}),\nExpected Velocity.Y: 0±0.05 (Got: {rb.linearVelocity.y})");

    }
    public IEnumerator WhileFlyingUp_DroneVertStabilization_Works(string drone_prefab_path)
    {
        //ARRANGE
        GameObject drone_instance = Instantiate(Resources.Load<GameObject>(drone_prefab_path), new Vector3(0,3,0), Quaternion.identity);
        Drone drone = drone_instance.GetComponent<Drone>();
        Rigidbody rb = drone.GetComponent<Rigidbody>();

        // ACT
        drone.constant_prop_activation_value = 6f;
        drone.ToggleConstantPropActivation(true);
        yield return new WaitForSeconds(1f);
        drone.ToggleConstantPropActivation(false);
        float expected_height = drone.transform.position.y;
        drone.ToggleVerticalStabilization(true, expected_height);
        yield return new WaitForSeconds(recovery_time);

        // ASSERT: Drone retains its height and has no vertical speed
        float current_height = drone.transform.position.y;
        bool isHeightCorrect = System.Math.Abs(current_height - expected_height) <= 0.5f;

        bool isVelocityCorrect = System.Math.Abs(rb.linearVelocity.y) <= 0.5f; 
        
        Assert.IsTrue(isHeightCorrect && isVelocityCorrect, 
        $"Drone Stabilization Failed!\nExpected height: {expected_height}±0.5 (Got: {current_height} Delta: {expected_height - current_height}),\nExpected Velocity.Y: 0±0.05 (Got: {rb.linearVelocity.y})");

    }
}