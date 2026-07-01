using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core
{

    [RequireComponent(typeof(Rigidbody))]
    public abstract class Drone : MonoBehaviour
    {
        [SerializeField] readonly float max_tilt = 1f;
        [SerializeField] GameObject center_of_mass;
        [SerializeField] public Gyroscope gyroscope;
        [SerializeField] public WeatherStation weather_station;
        protected Dictionary<string, Propeller> propellers;
        protected Rigidbody rb;
        protected float stasis_force = float.PositiveInfinity;
        protected bool vert_stabilization;
        protected bool constant_prop_activation;
        protected float target_v_stab_height;
        protected float constant_prop_activation_value = 10f;
        public bool hold_rotation;
        public Vector3 target_rotation;
        protected abstract Dictionary<string, float> SetDroneRotation(Vector3 target_angles);
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = center_of_mass.transform.localPosition;
        }
        protected virtual void Start()
        {
            Debug.Log("prop count: " + propellers.Count + " max_force: " + propellers["FL"].max_force + " mass: " + rb.mass + "\n" + "stasis force: " + stasis_force);
        }
        protected void Update()
        {
            if (Keyboard.current.commaKey.isPressed) ToggleVerticalStabilization(true, weather_station.GetHeight());
            if (Keyboard.current.periodKey.isPressed) ToggleVerticalStabilization(false);

            if (Keyboard.current.wKey.isPressed)      ToggleHoldRotation(true, new(0, 0,  max_tilt));
            else if (Keyboard.current.sKey.isPressed) ToggleHoldRotation(true, new(0, 0, -max_tilt));
            else if (Keyboard.current.aKey.isPressed) ToggleHoldRotation(true, new(-max_tilt, 0, 0));
            else if (Keyboard.current.dKey.isPressed) ToggleHoldRotation(true, new( max_tilt, 0, 0));
            //else ToggleHoldRotation(false, Vector3.zero);

            //if(Input.GetKeyDown(KeyCode.Semicolon)) {flight_target = Vector3.zero;  targeted_flight = true;}
            //if(Input.GetKeyDown(KeyCode.Quote)) targeted_flight = false;
        }
        protected void FixedUpdate()
        {
            Dictionary<string, float> activation_from_target_angle = SetDroneRotation(target_rotation);
            Dictionary<string, float> final_propeller_force = new();

            foreach(string propeller_name in propellers.Keys)
            {
                final_propeller_force[propeller_name] = 0;

                if (hold_rotation) final_propeller_force[propeller_name] += activation_from_target_angle[propeller_name];
                if (vert_stabilization) final_propeller_force[propeller_name] += VerticalStabilization(target_v_stab_height);
                propellers[propeller_name].SetPropellerForce(final_propeller_force[propeller_name]);
            }
        }
        protected static float FindStasisForce(int propellerCount, float mass, Vector3 gyroscopeAngles)
        {
            // 1. Calculate gravity and the weight that needs to be lifted
            float gravity = Mathf.Abs(Physics.gravity.y);
            float requiredLift = mass * gravity;

            // 2. Convert gyroscope degrees to radians for Unity's math functions
            float pitchRad = gyroscopeAngles.x * Mathf.Deg2Rad;
            float rollRad = gyroscopeAngles.z * Mathf.Deg2Rad;

            // 3. Calculate how much vertical lift is preserved at this specific tilt
            float cosPitch = Mathf.Cos(pitchRad);
            float cosRoll = Mathf.Cos(rollRad);
            float verticalEfficiency = cosPitch * cosRoll;

            // 4. Prevent division by zero if the drone is tilted 90 degrees or upside down
            if (verticalEfficiency <= 0.001f)
            {
                return float.PositiveInfinity;
            }

            // 5. Calculate total counter-gravity force and divide it among the propellers
            float totalForceNeeded = requiredLift / verticalEfficiency;
            float forcePerPropeller = totalForceNeeded / propellerCount;

            return Mathf.Max(forcePerPropeller, 0f);
        }
        public void ToggleVerticalStabilization(bool state, float target_height = -10f)
        {
            vert_stabilization = state;
            if (state) target_v_stab_height = target_height;
        }
        public void ToggleHoldRotation(bool state, Vector3 rotation)
        {
            hold_rotation = state;
            if (state) target_rotation = rotation; 
        }
        public void ToggleConstantPropActivation(bool state, float force = 0f)
        {
            constant_prop_activation = state;
            if (state) constant_prop_activation_value = force;
            else constant_prop_activation_value = 0f;
        }
        public float VerticalStabilization(float target_height)
        {
            Debug.Log(name + " target height: " + target_height);
            float height = weather_station.GetHeight();
            float target_force;
            float stasis_force = FindStasisForce(propellers.Count, rb.mass, gyroscope.GetReading());
            
            if (float.IsInfinity(height))
            {
                Debug.LogWarning(weather_station + " can't see the ground, lowering the drone");
                target_force = stasis_force * 0.9f;
                return target_force;

            }
            float offset = target_height - height;
            target_force = stasis_force + offset - rb.linearVelocity.y;
            Debug.Log("target force = " + target_force);
            if (target_force < 0) target_force = 0;

            return target_force;
        }
        public void ConstantPropActivation(float value)
        {//for testing 
            foreach (Propeller prop in propellers.Values) prop.SetPropellerForce(value);
        }
    }
}