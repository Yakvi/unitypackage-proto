# Overview

Collection of quick scripts, shaders, and materials for prototyping.

# Installation

If you're working with a git project, you can clone this project as a submodule. The snippet below presumes that your
your git repository is inside your Unity project:

```
~/[Your project git folder] git submodule add https://github.com/yakvi/unitypackage-proto Packages/com.venetstudio.proto  
```

# Contents

## Scripts

### Camera Controller

Adds WASD controls to the camera, as well as Q/E for rotation and wheel for zoom.

### Input Center

Event-based input processing. Provides events for:

* `static OnClick`: called when mouse 0 is released.
* `static OnRightClick`: called when mouse 1 is released.

Tracks and exposes input for:

* `static bool mousePosInvalid`: Validation of mouse position (whether or not mouse is in game bounds)
* `static Vector3 mousePos`: X and Y store screen mouse position, Z unused.
* `static Vector3 mousePosOnClickStart`: Cached `mousePos`, only updates when the mouse 0 gets pressed.
* `static float mouseWheel`: Mouse wheel delta from the last frame.
* `static Vector3 movement`: X and Z store horizontal/vertical axis. Y stores Q/E state.

The input center also allows custom hotkey processing. Keys can be bound using the `BindHotkey` and `UnbindHotkey`
methods. The user can specify if the hotkey should be processed on release on while it's pressed.

### Utility functions

All the utility functions are stored in the static class `VenetStudio.Utility`. As such, they can be shortcut if you
add `using static VenetStudio.Utility`.

* Camera (`MouseUtils.cs`)
    * `Camera GetMainCam()`: caches and returns the main camera.
    * `Transform GetMainCamTransform()`: caches and returns main camera transform.
    * `Vector3 GetCameraPos()`: returns the main camera position. The value is cached every frame.
* Mouse (`MouseUtils.cs`)
    * `bool IsMouseOverUi()`: Inverse shortcut for `EventSystem.current.IsPointerOverGameObject()`.
    * `Vector3 GetMousePhysicsPos(int LayerMask)`: returns 3D mouse position based on provided Physics mask. By default
      the mask is "Everything". The value is cached every frame.
        * Note: currently, only the first queried mask is cached and returned each frame.
    * `bool GetMousePhysicsHit(out RaycastHit hit, int layerMask)`: Same as `GetMousePhysicsPos`, more explicit.
      the `bool` value makes sure that the collision is valid and that there was no UI.
        * Note: currently, only the first queried mask is cached and returned each frame.
    * `static Vector3 GetMousePlanePos()`: Returns mouse position on the `up, 0` plane. The value is cached every frame.
    * `static bool TryGetWorldMousePlanePos(out Vector3 mousePos)`: Same as `GetMousePlanePos`, except it verifies if
      mouse is over UI.
* Time (`MouseUtils.cs`)
    * `static float GetDeltaTime()` Returns delta time. The value is cached every frame.
* Physics (`PhysicsUtils.cs`)
    * `bool Raycast(Vector3 position, Vector3 direction, float maxDistance, LayerMask mask)`: Currently, identical
      to `Physics.Raycast`.
    * `static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance, LayerMask mask)`: Currently, identical
      to `Physics.Raycast`.
    * `static int OverlapSphere(Vector3 center, float radius, Collider[] results, int mask = ~0)`: Currently, identical
      to `Physics.OverlapSphere`.
* Random number generation (`RandomUtils.cs`)
    * `float GetRandom(float min, float max)`: Currently, identical to `Random.Range`;

## Shaders

* `GridGlobal`: A shader that draws texture in a grid pattern in world space
* `GridObject`: A shader that draws texture in a grid pattern in object space
* `GlassGlobal`: A glassy shader, texture is projected in world space
* `GlassObject`: A glassy shader, texture is projected in object space

## Other

### Post-processing volume

Enabled components:

* Tonemapping
* Color Adjustments
* Bloom
* Vignette
* Film Grain
* Split Toning

### URP profile

Venet_High_URP profile is mostly identical to the default High URP profile. The only difference are some minor tweaks to
ambient occlusion. 