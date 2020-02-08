# Usage 

## Cameras

```c#
CamManager.cs
``` 
In charge of collecting all the cameras into a list, when you press the C button, it will change to the next camera.

```c#
Cam.cs
```
Attach this to a camera component to allow control over the camera. The camera component must be a child of some object (e.g. a planet). This script will make the camera follow the parent object (the planet). 

The script has multiple parameters in the inspector that can be toggled. 

Use WASD keys to rotate around the object the camera is attached to. Use + and - keys to zoom in or zoom out.
