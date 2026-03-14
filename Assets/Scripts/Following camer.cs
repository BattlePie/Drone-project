using UnityEngine;

public class Following_camera : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] [Tooltip("Время за которое камера долетает до объекта")]float time = 0.3f;
    [SerializeField] [Tooltip("Максималная скорость камеры")]float max_speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Quaternion target_rotation;
    Vector3 target_position;
    Quaternion relative_rotation;
    Vector3 offset_position;
    Vector3 velocity = Vector3.zero;
    void Start()
    {

        offset_position = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        target_position = target.transform.position + offset_position;
        transform.position = Vector3.SmoothDamp(transform.position, target_position, ref velocity, time, max_speed);
    }
}
