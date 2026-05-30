using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    public Vector3 GetReading()
    {
        Vector3 gyroRotation = transform.localEulerAngles;
        gyroRotation.x = Mathf.DeltaAngle(0, gyroRotation.x);
        gyroRotation.y = Mathf.DeltaAngle(0, gyroRotation.y);
        gyroRotation.z = Mathf.DeltaAngle(0, gyroRotation.z);
        return gyroRotation;
    }
}
