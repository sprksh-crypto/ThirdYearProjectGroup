# Usage 

## Plotter Python Script
Use this on a text file separated by lines. Run in terminal by locating to the directory and running the python script. Use commands ls and cd to get to directory. The script should be in the same folder as the text file containing the data.

Example:
```Python
python plotter.py Mercury.txt
```

## Cameras

```c#
CamManager.cs
``` 
In charge of collecting all the cameras into a list, when you press the C button, it will change to the next camera. 
Attach to an empty game object. 

```c#
CamMovement.cs
```
Attach this to a camera component to allow control over the camera. The camera component must be a child of some object (e.g. a planet). This script will make the camera follow the parent object (the planet). 

The script has multiple parameters in the inspector that can be toggled. 

Use WASD keys to rotate around the object the camera is attached to. Use + and - keys to zoom in or zoom out.


## Gravitational Objects
```c#
GravityBody.cs
```
Attach to a game object that has a rigid body component. 

If this body orbits another body, drag and drop that game object into "Orbit Target" in inspector. 

A semi-major axis can be specified in inspector. If left empty, it defaults to circular orbit (given that there is a target to orbit around).
