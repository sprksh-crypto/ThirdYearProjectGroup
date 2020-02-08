using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static Camera[] cameras;
    private int currentCameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        cameras = FindObjectsOfType<Camera>();
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    private void InputChangeCam()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            cameras[currentCameraIndex].gameObject.SetActive(false);
            currentCameraIndex += 1;
            if (currentCameraIndex == cameras.Length)
            {
                currentCameraIndex = 0;
            }
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputChangeCam();
    }
}
