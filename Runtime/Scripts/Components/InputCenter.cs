// #define USE_NEW_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine;

#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace VenetStudio
{
    public class InputCenter : MonoBehaviour
    {
        public static bool isGameRunning;

        public static event Action OnClick;
        public static event Action OnRightClick;

        private static Dictionary<KeyCode, Action> releaseHotkeyActions = new(101);
        private static Dictionary<KeyCode, Action> continuousHotkeyActions = new(101);
        
        public static bool BindHotkey(KeyCode key, Action action, bool continuousPress = false)
        {
            if (releaseHotkeyActions == null) releaseHotkeyActions = new Dictionary<KeyCode, Action>(101);
            if (continuousHotkeyActions == null) continuousHotkeyActions = new Dictionary<KeyCode, Action>(101);
            var dictionary = continuousPress ? continuousHotkeyActions : releaseHotkeyActions;
            var result = !dictionary.ContainsKey(key);
            if (result)
            {
                dictionary[key] = action;
            }

            return result;
        }

        public static void UnbindHotkey(KeyCode key, bool continuousPress = false)
        {
            var dictionary = continuousPress ? continuousHotkeyActions : releaseHotkeyActions;
            if (dictionary == null || !dictionary.ContainsKey(key)) return;
            
            dictionary.Remove(key);
        }

        public static bool mousePosInvalid;
        public static Vector3 mousePos;
        public static Vector3 mousePosOnClickStart;
        public static float mouseWheel;
        public static Vector3 movement;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            var input = FindObjectOfType<InputCenter>();
            if (input == null) input = new GameObject("Input Center", typeof(InputCenter)).GetComponent<InputCenter>();
            var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null) input.gameObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
        }

        private void Start()
        {
            isGameRunning = true;
        }

        private void LateUpdate()
        {
            UpdateMouse();
            
            UpdateKeyboard();
        }

#if USE_NEW_INPUT_SYSTEM
        private InputPlayerActions inputSystem;

        private void Awake()
        {
            inputSystem = new InputPlayerActions();
            inputSystem.Player.Enable();
        }

        private void UpdateKeyboard()
        {
            // Keyboard - Hits
            
            // Keyboard - Continuous
            var cameraMovement = inputSystem.Player.CameraMovement.ReadValue<Vector2>();
            movement.x = cameraMovement.x;
            movement.y = inputSystem.Player.CameraRotation.ReadValue<float>();
            movement.z = cameraMovement.y;
        }

        private void UpdateMouse()
        {
            if (!isGameRunning) return;
            RefreshMousePosition();
            if (mousePosInvalid) return; // Mouse outside the game window, do nothing

            if (inputSystem.Player.Action.WasPressedThisFrame()) mousePosOnClickStart = mousePos;
            if (inputSystem.Player.Action.WasReleasedThisFrame()) OnClick?.Invoke();
            if (inputSystem.Player.Context.WasReleasedThisFrame()) OnRightClick?.Invoke();
            mouseWheel = inputSystem.Player.CameraZoom.ReadValue<float>();
        }
#else
        private void UpdateKeyboard()
        {
            // Keyboard - Hotkeys
            foreach (var keyValuePair in releaseHotkeyActions)
            {
                if (Input.GetKeyUp(keyValuePair.Key)) keyValuePair.Value();
            }
            
            foreach (var keyValuePair in continuousHotkeyActions)
            {
                if (Input.GetKey(keyValuePair.Key)) keyValuePair.Value();
            }
            
            // Keyboard - 3D Movement
            movement.x = Input.GetAxis("Horizontal");
            movement.z = Input.GetAxis("Vertical");
            var movementUp = Input.GetKey(KeyCode.E);
            var movementDown = Input.GetKey(KeyCode.Q);
            if (movementDown != movementUp) movement.y = movementUp ? 1 : -1;
            else movement.y = 0;
        }

        private void UpdateMouse()
        {
            if (!isGameRunning) return;
            RefreshMousePosition();
            if (mousePosInvalid) return; // Mouse outside the game window, do nothing

            if (Input.GetMouseButtonDown(0)) mousePosOnClickStart = mousePos;
            if (Input.GetMouseButtonUp(0)) OnClick?.Invoke();
            if (Input.GetMouseButtonUp(1)) OnRightClick?.Invoke();
            mouseWheel = Input.mouseScrollDelta.y;
        }
#endif

        private void RefreshMousePosition()
        {
#if USE_NEW_INPUT_SYSTEM
            Vector3 mousePosition = Pointer.current != null ?
                Pointer.current.position.ReadValue() : 
                new Vector3(-1, -1, 0);
#else
            Vector3 mousePosition = Input.mousePosition;
#endif
            mousePosInvalid = (mousePosition.x < 0
                               || mousePosition.y < 0
                               || mousePosition.x > Screen.width
                               || mousePosition.y > Screen.height);
            if (!mousePosInvalid) mousePos = mousePosition;
        }
    }
}