using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour
{
    public float movementSpeed = 100f;  // Speed of the Spaceship, editable in inspector. 
    public float resetSpeed = 100f;     
    public float shiftSpeed = 150f;    
    public float controlSpeed = 50f;    // How fast turning controls work

    public float horizSensitivity = 2f;         // Horizontal sensistivity 
    public float vertSensitivity = 2f;          // Vertical sensistivity 
   

    private float yaw = 0f;     
    private float pitch = 0f;   

    public Animator getAnim; // For the animations used in flying

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }


    void Update()
    {
        yaw += horizSensitivity * Input.GetAxis("Mouse X");     
        pitch -= vertSensitivity * Input.GetAxis("Mouse Y");     

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);    


        //if statements to Check which button is pressed and let that determine how/where to move in space. 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = shiftSpeed;           
        }
        else
        {
            movementSpeed = resetSpeed;            
        }





        // Transform the position object to the forward position when key W,A,S or D is pressed

        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += transform.forward * Time.deltaTime * controlSpeed; 
            getAnim.SetBool("Down", true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= transform.right * Time.deltaTime * controlSpeed; 
            getAnim.SetBool("Left", true);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= transform.forward * Time.deltaTime * controlSpeed; 
            getAnim.SetBool("Up", true);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += transform.right * Time.deltaTime * controlSpeed;
            getAnim.SetBool("Right", true);
        }

        // For annimation 
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) 
        {
            getAnim.SetBool("Forward", true);   
            getAnim.SetBool("Down", false);
            getAnim.SetBool("Up", false);
            getAnim.SetBool("Left", false);
            getAnim.SetBool("Right", false);
        }
        else
            getAnim.SetBool("Forward", false);
    }
}
