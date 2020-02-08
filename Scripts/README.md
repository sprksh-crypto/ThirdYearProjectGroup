# Usage 

## Cameras

```c#
CamManager.cs
``` 
In charge of collecting all the cameras into a list, when you press the C button, it will change to the next camera.

```c#
Cam.cs
```
Attach this to a camera to allow control over the camera. The camera must be a child of some object. This script will make the camera follow the parent object. 

The script has multiple parameters in the inspector. 

Use WASD keys to rotate around the object the camera is attached to. Use + and - keys to zoom in or zoom out.
