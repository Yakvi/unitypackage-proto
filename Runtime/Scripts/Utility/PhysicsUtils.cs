using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public partial class Utility
    {
        public static bool Raycast(Vector3 position, Vector3 direction, float maxDistance, LayerMask mask) =>
            Physics.Raycast(position, direction, maxDistance, mask);

        public static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance, LayerMask mask) =>
            Physics.Raycast(ray, out hit, maxDistance, mask);

        public static int OverlapSphere(Vector3 center, float radius, Collider[] results, int mask = ~0) =>
            Physics.OverlapSphereNonAlloc(center, radius, results, mask);
    }

}