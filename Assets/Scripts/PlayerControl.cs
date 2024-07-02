using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private InputActionReference moveAction;
    public float Speed;
    Rigidbody rb;
    [SerializeField]
    float Angle;
    [SerializeField]
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();

        rb.velocity = (new Vector3(moveDirection.x, 0, moveDirection.y) * Speed);
        velocity = rb.velocity;
        RotateTowardsTarget();
    }

    /// <summary>
    /// rotation towards moving direction
    /// NEEDS TO BE SMOOTHENED
    /// </summary>
    void RotateTowardsTarget()
    {
        Angle = Vector3.Angle(rb.velocity, transform.forward);
        //if (Angle < 90)
        //{
            float smoothAngle = Mathf.LerpAngle(0, Angle, .6f);
            //Quaternion rotation = transform.rotation;
            //Transform newtransform = transform;
            //newtransform.Rotate(Vector3.up, smoothAngle);
            //transform.rotation = Quaternion.Slerp(rotation, newtransform.rotation, .9f);

            transform.Rotate(Vector3.up, smoothAngle);

        //}
        //else
        //{
        //    transform.rotation *= Quaternion.Euler(0, Angle, 0);
        //}

    }
}
