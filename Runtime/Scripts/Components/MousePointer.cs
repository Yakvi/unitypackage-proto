using System;
using UnityEngine;
using static VenetStudio.Utility;

namespace VenetStudio
{
    public class MousePointer : MonoBehaviour
    {
        public enum Mode
        {
            Plane,
            Collision
        }

        public Mode mode;
        
        private void Update()
        {
            switch (mode)
            {
                case Mode.Plane:
                    transform.position = GetMousePlanePos();
                    break;
                case Mode.Collision:
                    transform.position = GetMousePhysicsHit(out var hit) ? hit.point : GetMousePlanePos();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
