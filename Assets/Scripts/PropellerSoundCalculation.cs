using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PropellerSoundCalculation : MonoBehaviour 
{
    private Propeller propeller;
    private AudioSource audioSource;

    [Header("Pitch")]
    public float minPitch = 0.6f; 
    public float maxPitch = 3.0f;

    [Header("Volume (Normalized 0.0 to 1.0)")]
    public float minVolume = 0.2f; 
    public float maxVolume = 1.0f; 

    [Header("Master Loudness Ceiling")]
    [Range(0f, 1f)] public float masterVolumeCeiling = 0.2f; 

    [Header("Smoothing")]
    public float spinUpSmoothing = 50f;    // Maxed out to snap audio gates open instantly
    public float spinDownSmoothing = 1.5f; // Slow, heavy wind-down tail

    private float targetPitch;
    private float targetVolume;
    private float currentSmoothing;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        propeller = GetComponent<Propeller>();
        
        // 1. Keep the track constantly looping silently in the background 
        // to completely bypass Unity's internal software loading times.
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.pitch = minPitch;
        
        targetVolume = 0f;
        targetPitch = minPitch;

        audioSource.Play(); 
    }

    void Update() 
    {
        if (propeller == null) return;

        // 2. Read raw force demands. This hits completely before FixedUpdate computes the Sqrt math.
        bool isEngineRequested = propeller.curr_force > 0.0001f;

        if (isEngineRequested) 
        {
            currentSmoothing = spinUpSmoothing;

            // Map the audio ratio based on current force output relative to the maximum possible force
            float ratio = 0f;
            if (propeller.max_force > 0f)
            {
                ratio = Mathf.Clamp01(propeller.curr_force / propeller.max_force);
            }
            
            // Instant Igniter: If the engine is active, prevent it from dropping into absolute silence
            if (ratio < 0.15f)
            {
                ratio = 0.15f; 
            }

            targetPitch = Mathf.Lerp(minPitch, maxPitch, ratio);
            targetVolume = Mathf.Lerp(minVolume, maxVolume, ratio);
        }
        else 
        {
            // 3. Gracefully slide back down using your acoustic flywheel smoothing speed
            currentSmoothing = spinDownSmoothing;
            
            targetPitch = minPitch;
            targetVolume = 0f; 
        }

        // 4. Interpolate final values frame-by-frame
        audioSource.pitch = Mathf.MoveTowards(audioSource.pitch, targetPitch, Time.deltaTime * currentSmoothing * 3f);
        
        float calculatedVolume = Mathf.MoveTowards(audioSource.volume / masterVolumeCeiling, targetVolume, Time.deltaTime * currentSmoothing);
        audioSource.volume = calculatedVolume * masterVolumeCeiling;
    }
}
