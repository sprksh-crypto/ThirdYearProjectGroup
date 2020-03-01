using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    public float zoomSpeed = 20f;
    public static float rotateSpeed = 40f;

    private float zoomSpeedFactor = 1;
    private float zoomLimit;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private bool PlayerControllable = true;

    private float smoothSpeed = 0.25f;
    private Vector3 desiredPosition;
    private Transform target;
    private GravityBody targetGB;


    private void Start()
    {
        target = this.transform.parent;
        if (target == null)
        {
            PlayerControllable = false;
        }

        targetGB = target.GetComponent<GravityBody>();

        zoomLimit = 3 * targetGB.GetRadius() * target.transform.localScale.magnitude;

        if (offset == Vector3.zero)
        {
            offset = 3 * zoomLimit * new Vector3(-1, 0, -1);
        }
    }

    public void ZoomIn()
    {

        if ((target.position - transform.position).magnitude < zoomLimit * 6)
        {
            zoomSpeedFactor = 0.2f;
        }

        if ((target.position - transform.position).magnitude > zoomLimit)
        {
            offset -= zoomSpeed * zoomSpeedFactor * (transform.position - target.position).normalized;

        }
    }

    public void ZoomIn(float difference)
    {

        if ((target.position - transform.position).magnitude < zoomLimit * 6)
        {
            zoomSpeedFactor = 0.2f;
        }

        if ((target.position - transform.position).magnitude > zoomLimit)
        {
            offset -= difference * zoomSpeed * zoomSpeedFactor * (transform.position - target.position).normalized;

        }
    }

    public void ZoomOut()
    {
        if ((target.position - transform.position).magnitude > zoomLimit * 6)
        {
            zoomSpeedFactor = 1f;
        }
        offset += zoomSpeed * zoomSpeedFactor * (transform.position - target.position).normalized;
    }

    public void ZoomOut(float difference)
    {
        if ((target.position - transform.position).magnitude > zoomLimit * 6)
        {
            zoomSpeedFactor = 1f;
        }
        offset += difference * zoomSpeed * zoomSpeedFactor * (transform.position - target.position).normalized;
    }

    public void InputRotateAround(string direction)
    {
        Vector3 axis = Vector3.zero;

        switch (direction)
        {
            case "W":
                axis = Vector3.Cross((target.position - transform.position).normalized, Vector3.down);
                break;
            case "S":
                axis = Vector3.Cross((target.position - transform.position).normalized, Vector3.up);
                break;
            case "A":
                axis = Vector3.up;
                break;
            case "D":
                axis = Vector3.down;
                break;
            default:
                Debug.Log("Invalid case");
                break;
        }

        Quaternion camTurnAngle = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, axis);
        offset = camTurnAngle * offset;

    }

    public void TouchRotate(Vector2 touchMovement)
    {
        Vector3 xzAxis = Vector3.Cross(Vector3.up, (target.position - transform.position));
        Vector3 yAxis = Vector3.down;

        if (Mathf.Abs(transform.rotation.x) < 89.9)
        {
            offset = Quaternion.AngleAxis(-touchMovement.x * Time.deltaTime * 1440 / Screen.width, yAxis) * offset;
        }

        offset = Quaternion.AngleAxis(-touchMovement.y * Time.deltaTime * 1440 / Screen.width, xzAxis) * offset;

    }

    private void FixedUpdate()
    {
        if (PlayerControllable)
        {
            desiredPosition = target.position + offset;

            //// Interpolate to move to new position 
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Ensures that camera is pointing towards target
            transform.LookAt(target);
        }

    }

}
