
# Quick Blocking Generator

![alt text](https://raw.githubusercontent.com/Desayuno64/UnityQuickBlocking/main/QuickBlocking/Assets/Screenshots/ToolPreview.gif)

This is a simple spline based mesh blocking generator, it depends on the CinemachineSmoothPath or CinemachinePath,

![alt text](https://raw.githubusercontent.com/Desayuno64/UnityQuickBlocking/main/QuickBlocking/Assets/Screenshots/CinemachinePath.png)

Currently it does not support smooth blocking, only point based which gives you a blocky but userfull result.

To use it, just create a path, and add the script to a gameobject

Once you've created a path, update it through the "Update Mesh" in the three dots component, that will create the mesh and update the Mesh collider component ready to test.

![alt text](https://raw.githubusercontent.com/Desayuno64/UnityQuickBlocking/main/QuickBlocking/Assets/Screenshots/HowToUpdate.gif)

Some improvements I'm not so sure how to make yet are:

- Better triangulation method, right now every vertex is connected to point 0, which sometimes generates clipping.
- Adjustable resolution, right now there's no way to smooth borders by using the interpolation between points.
- UVs for the side and top parts of the mesh.

(feel free to contribute)

Project is set up in Unity 2021.3.21f URP 12
