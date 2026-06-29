using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental;

namespace Core
{

    [RequireComponent(typeof(Rigidbody))]
    public abstract class Drone : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Пропорции включения первичных vs вторичных пропеллеров, при 1 вторичные пропллеры включаются на полную мощность, при нуле включаются только первичные")]
        protected float tilt_ratio = 0.8f;
        [SerializeField] GameObject center_of_mass;
        [SerializeField] Gyroscope gyroscope;
        [SerializeField] public WeatherStation weather_station;
        float constant_prop_activation_value = 10f;
        protected Dictionary<string, Propeller> propellers;
        protected Rigidbody rb;
        protected float stasis_force = float.PositiveInfinity;
        protected float target_v_stab_height;
        protected bool vert_stabilization;
        protected bool constant_prop_activation;

        //bool targeted_flight = false;
        //Vector3 flight_target;
        protected abstract void ManualSteering();
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
            weather_station.GetAirDensity();
            if (Input.GetKeyDown(KeyCode.Comma)) ToggleVerticalStabilization(true);
            if (Input.GetKeyDown(KeyCode.Period)) ToggleVerticalStabilization(false);

            //if(Input.GetKeyDown(KeyCode.Semicolon)) {flight_target = transform.position;  targeted_flight = true;}
            //if(Input.GetKeyDown(KeyCode.Quote)) targeted_flight = false;
        }
        protected void FixedUpdate()
        {
            stasis_force = FindStasisForce(propellers.Count, propellers["FL"].max_force, rb.mass, gyroscope.GetReading());
            ManualSteering();
            if (constant_prop_activation) ConstantPropActivation(constant_prop_activation_value);
            if (vert_stabilization) VerticalStabilization(target_v_stab_height);
        }
        protected float FindStasisForce(int propellerCount, float maxForce, float mass, Vector3 gyroscopeAngles)
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
                return maxForce;
            }

            // 5. Calculate total counter-gravity force and divide it among the propellers
            float totalForceNeeded = requiredLift / verticalEfficiency;
            float forcePerPropeller = totalForceNeeded / propellerCount;

            // 6. Clamp the final value to individual motor limits
            return Mathf.Clamp(forcePerPropeller, 0f, maxForce);
        }
        public void ToggleVerticalStabilization(bool state)
        {
            switch (state)
            {
                case true:
                {
                    target_v_stab_height = weather_station.GetHeight(); 
                    vert_stabilization = true;
                    break;
                }
                case false:
                {
                    vert_stabilization = false;
                    break;
                }
            }
        }
        public void VerticalStabilization(float target_height)
        {
            Debug.Log(name + " target height: " + target_height);
            float height = weather_station.GetHeight();
            float target_force;
            if (float.IsInfinity(height))
            {
                Debug.LogWarning(weather_station + " can't see the ground, lowering the drone");
                target_force = stasis_force * 0.9f;
                foreach (Propeller prop in propellers.Values) prop.SetPropellerForce(target_force);
                return;

            }
            float offset = target_height - height;
            target_force = stasis_force + offset - rb.linearVelocity.y;
            Debug.Log("target force = " + target_force);
            if (target_force < 0) target_force = 0;
            foreach (Propeller prop in propellers.Values) prop.SetPropellerForce(target_force);
        }
        public void TiltStabilization()
        {
            throw new NotImplementedException();
        }
        public void ToggleConstantPropActivation(bool state, float force = 0f)
        {
            constant_prop_activation = state;
            switch (state)
            {
                case true:
                    constant_prop_activation_value = force;
                    break;
                case false:
                    constant_prop_activation_value = 0f;
                    break;
            }
        }
        public void ConstantPropActivation(float value)
        {//for testing 
            foreach (Propeller prop in propellers.Values) prop.SetPropellerForce(value);
        }
    }
}