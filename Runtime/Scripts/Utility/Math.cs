using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public partial class Utility
    {
        public static Vector3 GetOppositePoint(Vector3 c, Vector3 p) => (2 * c) - p;
    }
}