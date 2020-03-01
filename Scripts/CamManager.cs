using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamManager : MonoBehaviour
{
    public static List<Camera> cameras = new List<Camera>();
    public static Camera[] allCams;

    public static int currentCameraIndex = 0;

    private static CamMovement currentCamMovement;

    private CamManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        allCams = FindObjectsOfType<Camera>();
        for (int i = 0; i < allCams.Length; i++)
        {
            cameras.Add(allCams[i]);
        }
        UseCam(cameras[currentCameraIndex]);
        UpdateTextUI();

    }

    // Start is called before the first frame update
    void Start()
    {
        PrintCameraNames();
    }

    private static void UpdateTextUI()
    {
        InfoUI.SetCameraText(cameras[currentCameraIndex].transform.root.gameObject.name);
    }

    public static void UpdateTextUI(string text)
    {
        InfoUI.SetCameraText(text);
    }

    public static void RemoveCam(Camera cam)
    {
        int removeIndex = 0;
        foreach (Camera c in cameras)
        {
            if (c == cam)
            {
                if (currentCameraIndex == removeIndex)
                {
                    ChangeNextCam();
                    if (currentCameraIndex != 0)
                    {
                        currentCameraIndex--;
                    }
                }
                cameras.RemoveAt(removeIndex);
                return;
            }
            removeIndex++;
        }
        PrintCameraNames();
    }

    public static CamMovement GetCurrentCameraMover()
    {
        return cameras[currentCameraIndex].GetComponent<CamMovement>();
    }

    public static void UseCam(Camera cam)
    {
        bool foundCam = false;
        int indexer = 0;
        foreach (Camera c in cameras)
        {
            if (c == cam)
            {
                c.gameObject.SetActive(true);
                currentCameraIndex = indexer;
                foundCam = true;
                currentCamMovement = GetCurrentCameraMover();
            }

            else
            {
                c.gameObject.SetActive(false);
            }
            indexer++;
        }

        if (!foundCam)
        {
            throw new System.ArgumentException("Could not find camera: " + cam.gameObject.name);
        }
        return;
    }

    public static void ChangeNextCam()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Count)
        {
            currentCameraIndex = 0;
        }
        cameras[currentCameraIndex].gameObject.SetActive(true);
        currentCamMovement = GetCurrentCameraMover();
        UpdateTextUI();
        Debug.Log("Changing camera to " + cameras[currentCameraIndex].transform.root.gameObject.name);
    }

    public void ChangeNextCam_()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Count)
        {
            currentCameraIndex = 0;
        }
        cameras[currentCameraIndex].gameObject.SetActive(true);
        currentCamMovement = GetCurrentCameraMover();
        UpdateTextUI();

    }

    public void ZoomInCam()
    {
        currentCamMovement.ZoomIn();
    }

    public void ZoomOutCam()
    {
        currentCamMovement.ZoomOut();
    }

    static void PrintCameraNames()
    {
        List<string> camNames = new List<string>
        {
            "Camera list: "
        };
        foreach (Camera cam in cameras)
        {
            camNames.Add(cam.transform.root.name + " | ");
        }
        Debug.Log(string.Join("", camNames));
    }
}
