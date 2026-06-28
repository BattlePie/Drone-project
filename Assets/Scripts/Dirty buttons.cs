using System;
using UnityEngine;

public class Dirtybuttons : MonoBehaviour
{
    [SerializeField] Drone quadrocopter;
    [SerializeField] Drone hexacopter;
    [SerializeField] Drone octocopter;
    [SerializeField] GameObject wind;
    void OnGUI()
    {
    
        GUI.Box(new Rect(10, 10, 150, 140), "Drone Debug");

        if (GUI.Button(new Rect(20, 40, 130, 20), "Reset Quad"))
        {
            quadrocopter.transform.position = new Vector3(0, 1, -1);
            quadrocopter.transform.rotation = new Quaternion(0,0,0,0);
            quadrocopter.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            quadrocopter.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Drone reset");
        }
        if (GUI.Button(new Rect(20, 60, 130, 20), "Reset Octo"))
        {
            octocopter.transform.position = new Vector3(1, 1, 1);
            octocopter.transform.rotation = new Quaternion(0,0,0,0);
            octocopter.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            octocopter.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Drone reset");
        }
        if (GUI.Button(new Rect(20, 80, 130, 20), "Reset Hex"))
        {
            hexacopter.transform.position = new Vector3(-1, 1, 1);
            hexacopter.transform.rotation = new Quaternion(0,0,0,0);
            hexacopter.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hexacopter.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Drone reset");
        }
        if (GUI.Button(new Rect(20, 100, 130, 20), "Toggle wind"))
        {
            if (wind.activeInHierarchy)
            {
                wind.SetActive(false);
                Debug.Log("Wind off");
            }
            else
            {
                wind.SetActive(true);
                Debug.Log("Wind on");
            }

        }
    }

}
