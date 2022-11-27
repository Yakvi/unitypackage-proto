using System;
using Cinemachine;
using UnityEngine;
using static VenetStudio.Utility;

namespace VenetStudio
{
    public class CameraController : MonoBehaviour
    {
        public bool enableMovement = true;
        public bool enableRotation = true;
        public bool enableZoom = true;
        public float movementMetersPerSecond = 10f;
        public float yPlane;
        public float rotationDegreesPerSecond = 100f;

        public float zoomSpeed = 5f;

        public float angleMin = 2f;
        public float angleMax = 12f;

        public float distanceChangeStep = 0.5f;
        public float distanceMin = 5f;
        public float distanceMax = 20f;

        public CinemachineVirtualCamera vCam;
        public CinemachineFramingTransposer vCamTransposer;
        private Vector3 targetAngle;
        private float targetDistance;
        private bool shouldSprint;
        public Transform target;

        private void Start()
        {
            if (vCam == null) return;
            vCamTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (vCamTransposer == null) return;
            targetAngle = vCamTransposer.m_TrackedObjectOffset;
            targetDistance = vCamTransposer.m_CameraDistance;
        }

        private void OnEnable() => InputCenter.BindHotkey(KeyCode.LeftShift, SetSprint, true);
        private void OnDisable() => InputCenter.UnbindHotkey(KeyCode.LeftShift, true);

        private void Update()
        {
            if (enableMovement) UpdatePosition();
            if (enableRotation) UpdateRotation();
            if (enableZoom) UpdateZoom();

            shouldSprint = false;
        }

        private void UpdatePosition()
        {
            var movementMeters = movementMetersPerSecond * GetDeltaTime();

            if (target == null)
            {
                var moveInput = new Vector3(InputCenter.movement.x, 0, InputCenter.movement.z);
                var tr = transform;

                var moveVector = tr.right * moveInput.x + tr.forward * moveInput.z;
                if (shouldSprint) movementMeters *= 2;
                var targetPosition = moveVector * movementMeters;
                targetPosition += tr.position;
                targetPosition.y = yPlane;
                tr.position = targetPosition;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, movementMeters);
            }
        }

        private void UpdateRotation()
        {
            transform.eulerAngles +=
                new Vector3(0, -InputCenter.movement.y * rotationDegreesPerSecond * GetDeltaTime(), 0);
        }

        private void UpdateZoom()
        {
            if (vCamTransposer == null) return;

            var zoomChange = -InputCenter.mouseWheel;

            if ((zoomChange > 0.0f && targetAngle.y >= angleMax) || (zoomChange < 0.0f && targetDistance > distanceMin))
            {
                targetDistance += zoomChange;
                targetDistance = Mathf.Clamp(targetDistance, distanceMin, distanceMax);
            }
            else if (zoomChange != 0.0f)
            {
                targetAngle.y += zoomChange * distanceChangeStep;
                targetAngle.y = Mathf.Clamp(targetAngle.y, angleMin, angleMax);
            }

            var currentZoom = vCamTransposer.m_TrackedObjectOffset;
            var currentDistance = vCamTransposer.m_CameraDistance;
            vCamTransposer.m_TrackedObjectOffset =
                Vector3.Lerp(currentZoom, targetAngle, GetDeltaTime() * zoomSpeed);
            vCamTransposer.m_CameraDistance =
                Mathf.Lerp(currentDistance, targetDistance, GetDeltaTime() * zoomSpeed);
        }

        private void SetSprint() => shouldSprint = true;
    }
}