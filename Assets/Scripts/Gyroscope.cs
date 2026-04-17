using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    public Vector3 GetReading()
    {
        return transform.localEulerAngles;
    }
}
