using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public partial class Utility
    {
        public static bool Raycast(Vector3 position, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask mask) =>
            Physics.Raycast(position, direction, out hit, maxDistance, mask);

        public static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance, LayerMask mask) =>
            Physics.Raycast(ray, out hit, maxDistance, mask);

        public static int OverlapSphere(Vector3 center, float radius, Collider[] results, int mask = ~0) =>
            Physics.OverlapSphereNonAlloc(center, radius, results, mask);
        
        public static Collider[] OverlapSphere(Vector3 center, float radius, int mask = ~0) =>
            Physics.OverlapSphere(center, radius, mask);
    }

}