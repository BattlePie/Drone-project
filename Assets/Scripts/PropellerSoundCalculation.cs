using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public class PropellerSoundCalculation : MonoBehaviour
{
    [Header("References")]
    public Propeller propeller;

    private AudioSource audioSource;

    [Header("Pitch")]
    public float minPitch = 0.8f;
    public float maxPitch = 3.0f;

    [Header("Volume")]
    public float minVolume = 0.2f;
    public float maxVolume = 1.0f;

    [Header("Smoothing")]
    public float smoothing = 8f;

    private float targetPitch;
    private float targetVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (propeller == null)
            propeller = transform.gameObject.GetComponent<Propeller>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void Update()
    {
        if (propeller == null)
            return;

        float ratio = 0f;

        if (propeller.max_force > 0f)
            ratio = propeller.curr_force / propeller.max_force;

        ratio = Mathf.Clamp01(ratio);

        // Усиливаем разницу в звуке между малыми и большими оборотами
        ratio = Mathf.Pow(ratio, 0.5f);

        // Дополнительное повышение звука при манёврах
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed)
        {
            ratio += 0.25f;
        }

        ratio = Mathf.Clamp01(ratio);

        targetPitch = Mathf.Lerp(minPitch, maxPitch, ratio);
        targetVolume = Mathf.Lerp(minVolume, maxVolume, ratio);

        audioSource.pitch = Mathf.Lerp(
            audioSource.pitch,
            targetPitch,
            Time.deltaTime * smoothing
        );

        audioSource.volume = Mathf.Lerp(
            audioSource.volume,
            targetVolume,
            Time.deltaTime * smoothing
        );
    }
}