using UnityEngine;
using UnityEngine.EventSystems;

namespace VenetStudio
{
    public static partial class Utility
    {
        private static Camera mainCam;
        public static Camera GetMainCam()
        {
            if (mainCam == null && Camera.main != null) mainCam = Camera.main;
            return mainCam;
        }
        
        private static Transform mainCamTransform;
        public static Transform GetMainCamTransform()
        {
            if (mainCamTransform == null) mainCamTransform = GetMainCam().transform;
            return mainCamTransform;
        }
        
        private static readonly CachedData<Vector3> CameraPos = new ();
        public static Vector3 GetCameraPos()
        {
            if (!CameraPos.IsRelevant())
            {
                CameraPos.value = GetMainCamTransform().position;
            }

            return CameraPos.value;
        }
        
        public static bool IsMouseOverUi() => EventSystem.current.IsPointerOverGameObject();
        
        public static Vector3 GetMousePhysicsPos(int layerMask = ~0)
        {
            GetMousePhysicsHit(out var hit, layerMask);
            return hit.point;
        }

        // TODO(yakvi): This currently only works for one and only mask per frame. Expand MouseHit? 
        private static readonly CachedData<RaycastHit> MouseHit = new ();
        public static bool GetMousePhysicsHit(out RaycastHit hit, int layerMask = ~0)
        {
            if (!MouseHit.IsRelevant())
            {
                var ray = GetMainCam().ScreenPointToRay(InputCenter.mousePos);
                Raycast(ray, out MouseHit.value, float.MaxValue, layerMask);
            }

            hit = MouseHit.value;
            return MouseHit.value.transform != null && !IsMouseOverUi();
        }

        private static readonly CachedData<Vector3> MousePlanePos = new ();
        private static Plane mousePlane = new (Vector3.up, Vector3.zero);
        public static Vector3 GetMousePlanePos()
        {
            if (!MousePlanePos.IsRelevant())
            {
                var ray = GetMainCam().ScreenPointToRay(InputCenter.mousePos);
                if (mousePlane.Raycast(ray, out var collisionDistance))
                {
                    // Honestly, I don't think why this should ever fail, it's an infinite plane
                    MousePlanePos.value = ray.GetPoint(collisionDistance);
                }
            }
        
            return MousePlanePos.value;
        }

        public static bool TryGetWorldMousePlanePos(out Vector3 mousePos)
        {
            mousePos = GetMousePlanePos();
            return !IsMouseOverUi();
        }

        private static readonly CachedData<float> DeltaTime = new ();
        public static float GetDeltaTime()
        {
            if (!DeltaTime.IsRelevant()) DeltaTime.value = Time.deltaTime;
            return DeltaTime.value;
        }
    }
}