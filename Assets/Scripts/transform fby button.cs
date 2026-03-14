using System.Collections.Generic;
using UnityEngine;

public class transformbybutton : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rot_speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Time.deltaTime * transform.forward;
        }
         if (Input.GetKey(KeyCode.A))
        {
            transform.position += speed * Time.deltaTime * -transform.right;
        }
         if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * Time.deltaTime * -transform.forward;
        }
         if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * Time.deltaTime * transform.right;
        }
         if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, Time.deltaTime * -rot_speed, 0);
        }
         if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, Time.deltaTime * rot_speed, 0);
        }
    }
}
