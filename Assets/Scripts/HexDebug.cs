using UnityEngine;

public class HexDebug : MonoBehaviour
{
    [SerializeField] Hexocopter drone;
    [SerializeField] private float L_force;
    [SerializeField] private float FL_force;
    [SerializeField] private float FR_force;
    [SerializeField] private float R_force;
    [SerializeField] private float BL_force;
    [SerializeField] private float BR_force;
    Propeller L,R,FL,FR,BL,BR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
void Awake()
    {   
        FL = drone.FL_propeller.GetComponent<Propeller>();
        FR = drone.FR_propeller.GetComponent<Propeller>();
        BL = drone.BL_propeller.GetComponent<Propeller>();
        BR = drone.BR_propeller.GetComponent<Propeller>();
        R = drone.R_propeller.GetComponent<Propeller>();
        L = drone.L_propeller.GetComponent<Propeller>();
    }

    // Update is called once per frame
    void Update()
    {
        FL_force = FL.curr_force;
        FR_force = FR.curr_force;
        BL_force = BL.curr_force;
        BR_force = BR.curr_force;
        L_force = L.curr_force;
        R_force = R.curr_force;
    }
}
