using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// MirageAR Editor Emulator
    /// </summary>
    public class MirageAR_EditorEmulator : MonoBehaviour
    {

#if UNITY_EDITOR

        private const string AXIS_MOUSE_X = "Mouse X";
        private const string AXIS_MOUSE_Y = "Mouse Y";      

        // Use mouse to emulate head in the editor.
        private static float mouseX = 0;
        private static float mouseY = 0;
        private static float mouseZ = 0;

        private GameObject emptyChild;       

        // Use mouse to emulate controller in the editor
        private static float controllerMouseX;
        private static float controllerMouseY;

        // The Simulate Human Height,the actual Height is Between 0-defaultHeight
        private static float defaultHeight=1.6f;

        // Samulate Man Move Speed
        private static float moveSpeed = 0.05f;
        // Samulate Man Squat Speed
        private static float downSpeed = 0.02f;

        public static Vector3 inputPosition { get; private set; }
        public static Quaternion inputRotation { get; private set; }

        public static Vector3 controllerPosition { get; private set; }
        public static Quaternion controllerRotation { get; private set; }


        #region Unity Method       

        private void Awake()
        {
            //Create [EmptyChild] as Child to get Position and Rotation
            emptyChild = new GameObject("[EmptyChild]");
            emptyChild.transform.SetParent(this.gameObject.transform);
            emptyChild.transform.localPosition =new Vector3(0.2f,-0.15f,0.5f);
            emptyChild.transform.localEulerAngles=Vector3.zero;
        }

        private void Update()
        {
            bool rolled = false;
            bool controllerRoted = false;

            if (CanChangeYawPitch())
            {
                mouseX += Input.GetAxis(AXIS_MOUSE_X) * 5;
                if (mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if (mouseX > 180)
                {
                    mouseX -= 360;
                }
                mouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 2.4f;
                mouseY = Mathf.Clamp(mouseY, -85, 85);
            }
            else if (CanChangeRoll())
            {
                rolled = true;
                mouseZ += Input.GetAxis(AXIS_MOUSE_X) * 5;
                mouseZ = Mathf.Clamp(mouseZ, -85, 85);
            }
            else if (CanControllerInput())
            {
                controllerRoted = true;
                controllerMouseX += Input.GetAxis(AXIS_MOUSE_X) * 5;
                controllerMouseX = Mathf.Clamp(controllerMouseX,-85,85);
                controllerMouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 2.4f;
                controllerMouseY = Mathf.Clamp(controllerMouseY, -75, 75);
            }

            
            if (!rolled)
            {
                // People don't usually leave their heads tilted to one side for long.
                mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
            }

            if (!controllerRoted)
            {
                controllerMouseX = Mathf.Lerp(controllerMouseX,0,Time.deltaTime/(Time.deltaTime+0.1f));
                controllerMouseY = Mathf.Lerp(controllerMouseY, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
            }
            
            // Calc Target Hmd Position
            if (!CanChangeYawPitch()&&!CanChangeRoll()&&!CanControllerInput())
            {
                Vector3 value= GetMoveInput();
                inputPosition = new Vector3(value.x * moveSpeed,  value.y *downSpeed, value.z * moveSpeed);
            }

            // Update Hmd Position And Rotation 
            inputRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);            
            transform.localRotation = inputRotation;
            transform.Translate(inputPosition);

            // Update Controller Position And Rotation
            emptyChild.transform.localRotation = Quaternion.Euler(controllerMouseY,controllerMouseX,0);
            controllerPosition = emptyChild.transform.position;
            controllerRotation = emptyChild.transform.rotation;            
        }

        #endregion  //Unity Method

        #region Private Method

        private bool CanControllerInput()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }

        private bool CanChangeYawPitch()
        {           
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        }

        private bool CanChangeRoll()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }

        /// <summary>
        /// Keyboard "ASDWQE" represent:Left,Back,Right,Forward,Down,Up
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMoveInput()
        {
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                p_Velocity += new Vector3(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.E))
            {
                p_Velocity += new Vector3(0, 1, 0);
            }

            return p_Velocity;
        }

        #endregion  //Private Method

#endif //UNITY_EDITOR

    }

}

