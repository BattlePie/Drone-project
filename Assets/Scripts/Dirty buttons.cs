using System;
using UnityEngine;

public class Dirtybuttons : MonoBehaviour
{
    [SerializeField] GameObject quadrocopter;
    void OnGUI()
    {
    
        GUI.Box(new Rect(10, 10, 150, 100), "Drone Debug");

        if (GUI.Button(new Rect(20, 40, 130, 20), "Reset Drone"))
        {
            quadrocopter.transform.position = new Vector3(0, 1, -1);
            quadrocopter.transform.rotation = new Quaternion(0,0,0,0);
            Debug.Log("Drone reset");
        }
    }

}
