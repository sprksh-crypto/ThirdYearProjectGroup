using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float smoothSpeed = 0.25f;
    public float zoomSpeed = 10f;
    public float rotateSpeed = 20f;
    public Vector3 offset = new Vector3(-100, 0, -100);
    public bool PlayerControllable = true;
    public int rank;
    private Vector3 desiredPosition;
    private Transform target;

    private void Start()
    {
        target = this.transform.parent;
    }

    private void InputZoom()
    {
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals))
        {
            offset += zoomSpeed * (target.position - transform.position).normalized;
        }
        else if (Input.GetKey(KeyCode.Minus))
        {
            offset -= zoomSpeed * (target.position - transform.position).normalized;
        }
    }

    private void InputRotateAround()
    {
        Vector3 axis = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            axis = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            axis = Vector3.up;
        }
        if (Input.GetKey(KeyCode.W))
        {
            axis = Vector3.Cross((target.position - transform.position).normalized, Vector3.down);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            axis = Vector3.Cross((target.position - transform.position).normalized, Vector3.up);
        }

        if (axis != Vector3.zero)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, axis);
            offset = camTurnAngle * offset;
        } 
    }

    private void FixedUpdate()
    {
        if (PlayerControllable)
        {
            InputZoom();

            InputRotateAround();
        }

        desiredPosition = target.position + offset;

        // Interpolate to move to new position 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Ensures that camera is pointing towards target
        transform.LookAt(target);
    }

}
