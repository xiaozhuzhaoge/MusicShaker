// Mirage Controller|SDK_Mirage|004

namespace VRTK
{
#if VRTK_DEFINE_SDK_MIRAGE
    using UnityEngine;
    using System.Collections.Generic;
    using Ximmerse.Vision;
    using Ximmerse.Vision.Internal;
    using Ximmerse.InputSystem;
#endif

    /// <summary>
    /// The Mirage Controller SDK script provides a bridge to SDK methods that deal with the input devices.
    /// </summary>
    [SDK_Description(typeof(SDK_MirageSystem))]
    public class SDK_MirageController
#if VRTK_DEFINE_SDK_MIRAGE
        : SDK_BaseController
#else
        : SDK_FallbackController
#endif
    {
#if VRTK_DEFINE_SDK_MIRAGE
        protected GameObject controller;

        protected Vector3 prevPosition;
        protected Vector3 prevRotation;
        protected Vector3 velocity = Vector3.zero;
        protected Vector3 angularVelocity = Vector3.zero;
        protected Peripheral peripheral;
        /// <summary>
        /// The ProcessUpdate method enables an SDK to run logic for every Unity Update
        /// </summary>
        /// <param name="controllerReference">The reference for the controller.</param>
        /// <param name="options">A dictionary of generic options that can be used to within the update.</param>
        public override void ProcessUpdate(VRTK_ControllerReference controllerReference, Dictionary<string, object> options)
        {
            if (controller == null)
            {
                return;
            }

            velocity = (controller.transform.position - prevPosition) / Time.deltaTime;

            angularVelocity = Quaternion.FromToRotation(prevRotation, controller.transform.eulerAngles).eulerAngles / Time.deltaTime;

            prevPosition = controller.transform.position;
            prevRotation = controller.transform.rotation.eulerAngles;
        }

        /// <summary>
        /// The ProcessFixedUpdate method enables an SDK to run logic for every Unity FixedUpdate
        /// </summary>
        /// <param name="controllerReference">The reference for the controller.</param>
        /// <param name="options">A dictionary of generic options that can be used to within the fixed update.</param>
        public override void ProcessFixedUpdate(VRTK_ControllerReference controllerReference, Dictionary<string, object> options)
        {
        }

        /// <summary>
        /// The GetCurrentControllerType method returns the current used ControllerType based on the SDK and headset being used.
        /// </summary>
        /// <returns>The ControllerType based on the SDK and headset being used.</returns>
        public override ControllerType GetCurrentControllerType()
        {
            return ControllerType.Mirage_Controller;
        }

        /// <summary>
        /// The GetControllerDefaultColliderPath returns the path to the prefab that contains the collider objects for the default controller of this SDK.
        /// </summary>
        /// <param name="hand">The controller hand to check for</param>
        /// <returns>A path to the resource that contains the collider GameObject.</returns>
        public override string GetControllerDefaultColliderPath(ControllerHand hand)
        {
            return "ControllerColliders/Fallback";
        }

        /// <summary>
        /// The GetControllerElementPath returns the path to the game object that the given controller element for the given hand resides in.
        /// </summary>
        /// <param name="element">The controller element to look up.</param>
        /// <param name="hand">The controller hand to look up.</param>
        /// <param name="fullPath">Whether to get the initial path or the full path to the element.</param>
        /// <returns>A string containing the path to the game object that the controller element resides in.</returns>
        public override string GetControllerElementPath(ControllerElements element, ControllerHand hand, bool fullPath = false)
        {
            //EDIT:Set self define tips path
            //TODO: Use Gvr's tooltips or add an attach object ourselves
            string dd = "Root/Tootips/";
            string touchpad = dd + "Touchpad";
            string trigger = dd + "Trigger";
            string app = dd + "AppBtn";
            string home = dd + "HomeBtn";

            switch (element)
            {
                case ControllerElements.AttachPoint:
                    return dd; //TODO: attach point at tip of controller?
                case ControllerElements.Touchpad:
                    return touchpad;
                case ControllerElements.Trigger:
                    return trigger;
                case ControllerElements.ButtonOne:
                    return app;
                case ControllerElements.ButtonTwo:
                    return home;
                default:
                    return dd;
            }
        }

        /// <summary>
        /// The GetControllerIndex method returns the index of the given controller.
        /// </summary>
        /// <param name="controller">The GameObject containing the controller.</param>
        /// <returns>The index of the given controller.</returns>
        public override uint GetControllerIndex(GameObject controller)
        {
            return (CheckActualOrScriptAliasControllerIsRightHand(controller) ? 1 : uint.MaxValue);
        }

        /// <summary>
        /// The GetControllerByIndex method returns the GameObject of a controller with a specific index.
        /// </summary>
        /// <param name="index">The index of the controller to find.</param>
        /// <param name="actual">If true it will return the actual controller, if false it will return the script alias controller GameObject.</param>
        /// <returns></returns>
        public override GameObject GetControllerByIndex(uint index, bool actual = false)
        {
            return (index == 1 ? controller : null);
        }

        /// <summary>
        /// The GetControllerOrigin method returns the origin of the given controller.
        /// </summary>
        /// <param name="controllerReference">The reference to the controller to retrieve the origin from.</param>
        /// <returns>A Transform containing the origin of the controller.</returns>
        public override Transform GetControllerOrigin(VRTK_ControllerReference controllerReference)
        {
            return controllerReference.actual.transform;
        }

        /// <summary>
        /// The GenerateControllerPointerOrigin method can create a custom pointer origin Transform to represent the pointer position and forward.
        /// </summary>
        /// <param name="parent">The GameObject that the origin will become parent of. If it is a controller then it will also be used to determine the hand if required.</param>
        /// <returns>A generated Transform that contains the custom pointer origin.</returns>
        public override Transform GenerateControllerPointerOrigin(GameObject parent)
        {
            return null;
        }

        /// <summary>
        /// The GetControllerLeftHand method returns the GameObject containing the representation of the left hand controller.
        /// </summary>
        /// <param name="actual">If true it will return the actual controller, if false it will return the script alias controller GameObject.</param>
        /// <returns>The GameObject containing the left hand controller.</returns>
        public override GameObject GetControllerLeftHand(bool actual = false)
        {
            return null;
        }

        /// <summary>
        /// The GetControllerRightHand method returns the GameObject containing the representation of the right hand controller.
        /// </summary>
        /// <param name="actual">If true it will return the actual controller, if false it will return the script alias controller GameObject.</param>
        /// <returns>The GameObject containing the right hand controller.</returns>
        public override GameObject GetControllerRightHand(bool actual = false)
        {
            controller = GetSDKManagerControllerRightHand(actual);
            if ((controller == null) && actual)
            {
                controller = VRTK_SharedMethods.FindEvenInactiveGameObject<VisionSDK>().transform.Find("Controller").gameObject;
                //controller = GameObject.Find("Controller");
            }
            if (controller != null)
            {
                prevPosition = controller.transform.position;
                prevRotation = controller.transform.rotation.eulerAngles;
            }
            return controller;
        }

        /// <summary>
        /// The IsControllerLeftHand/1 method is used to check if the given controller is the the left hand controller.
        /// </summary>
        /// <param name="controller">The GameObject to check.</param>
        /// <returns>Returns true if the given controller is the left hand controller.</returns>
        public override bool IsControllerLeftHand(GameObject controller)
        {
            return false;
        }

        /// <summary>
        /// The IsControllerRightHand/1 method is used to check if the given controller is the the right hand controller.
        /// </summary>
        /// <param name="controller">The GameObject to check.</param>
        /// <returns>Returns true if the given controller is the right hand controller.</returns>
        public override bool IsControllerRightHand(GameObject controller)
        {
            return true;
        }

        /// <summary>
        /// The IsControllerLeftHand/2 method is used to check if the given controller is the the left hand controller.
        /// </summary>
        /// <param name="controller">The GameObject to check.</param>
        /// <param name="actual">If true it will check the actual controller, if false it will check the script alias controller.</param>
        /// <returns>Returns true if the given controller is the left hand controller.</returns>
        public override bool IsControllerLeftHand(GameObject controller, bool actual)
        {
            return false;
        }

        /// <summary>
        /// The IsControllerRightHand/2 method is used to check if the given controller is the the right hand controller.
        /// </summary>
        /// <param name="controller">The GameObject to check.</param>
        /// <param name="actual">If true it will check the actual controller, if false it will check the script alias controller.</param>
        /// <returns>Returns true if the given controller is the right hand controller.</returns>
        public override bool IsControllerRightHand(GameObject controller, bool actual)
        {
            return true;
        }

        /// <summary>
        /// The GetControllerModel method returns the model alias for the given GameObject.
        /// </summary>
        /// <param name="controller">The GameObject to get the model alias for.</param>
        /// <returns>The GameObject that has the model alias within it.</returns>
        public override GameObject GetControllerModel(GameObject controller)
        {
            return GetControllerModelFromController(controller);
        }

        /// <summary>
        /// The GetControllerModel method returns the model alias for the given controller hand.
        /// </summary>
        /// <param name="hand">The hand enum of which controller model to retrieve.</param>
        /// <returns>The GameObject that has the model alias within it.</returns>
        public override GameObject GetControllerModel(ControllerHand hand)
        {
            GameObject model = GetSDKManagerControllerModelForHand(hand);
            if (model == null)
            {
                GameObject controller = null;
                switch (hand)
                {
                    case ControllerHand.Left:
                        controller = GetControllerLeftHand(true);
                        break;
                    case ControllerHand.Right:
                        controller = GetControllerRightHand(true);
                        break;
                }

                if (controller != null)
                {
                    model = controller;
                }
            }
            return model;
        }

        /// <summary>
        /// The GetControllerRenderModel method gets the game object that contains the given controller's render model.
        /// </summary>
        /// <param name="controllerReference">The reference to the controller to check.</param>
        /// <returns>A GameObject containing the object that has a render model for the controller.</returns>
        public override GameObject GetControllerRenderModel(VRTK_ControllerReference controllerReference)
        {
            return controllerReference.model;
        }

        /// <summary>
        /// The SetControllerRenderModelWheel method sets the state of the scroll wheel on the controller render model.
        /// </summary>
        /// <param name="renderModel">The GameObject containing the controller render model.</param>
        /// <param name="state">If true and the render model has a scroll wheen then it will be displayed, if false then the scroll wheel will be hidden.</param>
        public override void SetControllerRenderModelWheel(GameObject renderModel, bool state)
        {
        }

        /// <summary>
        /// The HapticPulse/2 method is used to initiate a simple haptic pulse on the tracked object of the given controller reference.
        /// </summary>
        /// <param name="controllerReference">The reference to the tracked object to initiate the haptic pulse on.</param>
        /// <param name="strength">The intensity of the rumble of the controller motor. `0` to `1`.</param>
        public override void HapticPulse(VRTK_ControllerReference controllerReference, float strength = 0.5f)
        {
        }

        /// <summary>
        /// The HapticPulse/2 method is used to initiate a haptic pulse based on an audio clip on the tracked object of the given controller reference.
        /// </summary>
        /// <param name="controllerReference">The reference to the tracked object to initiate the haptic pulse on.</param>
        /// <param name="clip">The audio clip to use for the haptic pattern.</param>
        public override bool HapticPulse(VRTK_ControllerReference controllerReference, AudioClip clip)
        {
            //Return true so it just always prevents doing a fallback routine.
            return true;
        }

        /// <summary>
        /// The GetHapticModifiers method is used to return modifiers for the duration and interval if the SDK handles it slightly differently.
        /// </summary>
        /// <returns>An SDK_ControllerHapticModifiers object with a given `durationModifier` and an `intervalModifier`.</returns>
        public override SDK_ControllerHapticModifiers GetHapticModifiers()
        {
            return new SDK_ControllerHapticModifiers();
        }

        /// <summary>
        /// The GetVelocity method is used to determine the current velocity of the tracked object on the given controller reference.
        /// </summary>
        /// <param name="controllerReference">The reference to the tracked object to check for.</param>
        /// <returns>A Vector3 containing the current velocity of the tracked object.</returns>
        public override Vector3 GetVelocity(VRTK_ControllerReference controllerReference)
        {
            return velocity;
        }

        /// <summary>
        /// The GetAngularVelocity method is used to determine the current angular velocity of the tracked object on the given controller reference.
        /// </summary>
        /// <param name="controllerReference">The reference to the tracked object to check for.</param>
        /// <returns>A Vector3 containing the current angular velocity of the tracked object.</returns>
        public override Vector3 GetAngularVelocity(VRTK_ControllerReference controllerReference)
        {
            return angularVelocity;
        }

        /// <summary>
        /// The IsTouchpadStatic method is used to determine if the touchpad is currently not being moved.
        /// </summary>
        /// <param name="currentAxisValues"></param>
        /// <param name="previousAxisValues"></param>
        /// <param name="compareFidelity"></param>
        /// <returns>Returns true if the touchpad is not currently being touched or moved.</returns>
        public override bool IsTouchpadStatic(bool isTouched, Vector2 currentAxisValues, Vector2 previousAxisValues, int compareFidelity)
        {
            return (!isTouched || VRTK_SharedMethods.Vector2ShallowCompare(currentAxisValues, previousAxisValues, compareFidelity));
        }

        /// <summary>
        /// The GetButtonAxis method retrieves the current X/Y axis values for the given button type on the given controller reference.
        /// </summary>
        /// <param name="buttonType">The type of button to check for the axis on.</param>
        /// <param name="controllerReference">The reference to the controller to check the button axis on.</param>
        /// <returns>A Vector2 of the X/Y values of the button axis. If no axis values exist for the given button, then a Vector2.Zero is returned.</returns>
        public override Vector2 GetButtonAxis(ButtonTypes buttonType, VRTK_ControllerReference controllerReference)
        {
#if !UNITY_ANDROID || UNITY_EDITOR
            return Vector2.zero;
#else
            //@EDIT:Make Sure VisionSDK Instance is Inited
            if (!VisionSDK.Instance.Inited)
            {
                return Vector2.zero;
            }

            if (peripheral == null)
            {
                peripheral = VisionSDK.Instance.Connections.GetPeripheral("XCobra-0");

                if (peripheral == null)
                {
                    return Vector2.zero;
                }
                else
                {
                    return peripheral.ControllerInput.touchPos;
                }
            }
            else
            {
                return peripheral.ControllerInput.touchPos;
            }         
#endif

        }

        /// <summary>
        /// The GetButtonHairlineDelta method is used to get the difference between the current button press and the previous frame button press.
        /// </summary>
        /// <param name="buttonType">The type of button to get the hairline delta for.</param>
        /// <param name="controllerReference">The reference to the controller to get the hairline delta for.</param>
        /// <returns>The delta between the button presses.</returns>
        public override float GetButtonHairlineDelta(ButtonTypes buttonType, VRTK_ControllerReference controllerReference)
        {
            return 0f;
        }

        /// <summary>
        /// The GetControllerButtonState method is used to determine if the given controller button for the given press type on the given controller reference is currently taking place.
        /// </summary>
        /// <param name="buttonType">The type of button to check for the state of.</param>
        /// <param name="pressType">The button state to check for.</param>
        /// <param name="controllerReference">The reference to the controller to check the button state on.</param>
        /// <returns>Returns true if the given button is in the state of the given press type on the given controller reference.</returns>
        public override bool GetControllerButtonState(ButtonTypes buttonType, ButtonPressTypes pressType, VRTK_ControllerReference controllerReference)
        {            
            //@EDIT:Make Sure VisionSDK Instance is Inited
            if (!VisionSDK.Instance.Inited)
            {
                return false;
            }
            
            switch (buttonType)
            {
                case ButtonTypes.Touchpad:
#if UNITY_EDITOR
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return Input.GetKeyDown(KeyCode.Mouse0);
                        case ButtonPressTypes.Press:
                            return Input.GetKey(KeyCode.Mouse0);
                        case ButtonPressTypes.PressUp:
                            return Input.GetKeyUp(KeyCode.Mouse0);
                    }
#else
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return VisionSDK.Instance.Input.GetButtonDown(GetControllerPeripheral(),ButtonType.Touchpad);
                        case ButtonPressTypes.Press:
                            return VisionSDK.Instance.Input.GetButtonPressed(GetControllerPeripheral(), ButtonType.Touchpad);
                        case ButtonPressTypes.PressUp:
                            return VisionSDK.Instance.Input.GetButtonUp(GetControllerPeripheral(), ButtonType.Touchpad);
                    }
#endif
                    break;
                case ButtonTypes.Trigger:
#if UNITY_EDITOR                    
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return Input.GetKeyDown(KeyCode.Space);
                        case ButtonPressTypes.Press:
                            return Input.GetKey(KeyCode.Space);
                        case ButtonPressTypes.PressUp:
                            return Input.GetKeyUp(KeyCode.Space);
                    }
#else
                    switch (pressType)
                    {
                       case ButtonPressTypes.PressDown:
                            return VisionSDK.Instance.Input.GetButtonDown(GetControllerPeripheral(),ButtonType.Trigger);
                        case ButtonPressTypes.Press:
                            return VisionSDK.Instance.Input.GetButtonPressed(GetControllerPeripheral(), ButtonType.Trigger);
                        case ButtonPressTypes.PressUp:
                            return VisionSDK.Instance.Input.GetButtonUp(GetControllerPeripheral(), ButtonType.Trigger);
                    }
#endif
                    break;
                case ButtonTypes.ButtonOne://定义为APP键
#if UNITY_EDITOR
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return Input.GetKeyDown(KeyCode.Mouse1);
                        case ButtonPressTypes.Press:
                            return Input.GetKey(KeyCode.Mouse1);
                        case ButtonPressTypes.PressUp:
                            return Input.GetKeyUp(KeyCode.Mouse1);
                    }
#else
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return VisionSDK.Instance.Input.GetButtonDown(GetControllerPeripheral(),ButtonType.App);
                        case ButtonPressTypes.Press:
                            return VisionSDK.Instance.Input.GetButtonPressed(GetControllerPeripheral(), ButtonType.App);
                        case ButtonPressTypes.PressUp:
                            return VisionSDK.Instance.Input.GetButtonUp(GetControllerPeripheral(), ButtonType.App);
                    }
#endif
                    break;
                case ButtonTypes.ButtonTwo://定义为Home键
#if UNITY_EDITOR
                    switch (pressType)
                    {
                        case ButtonPressTypes.PressDown:
                            return Input.GetKeyDown(KeyCode.Mouse2);
                        case ButtonPressTypes.Press:
                            return Input.GetKey(KeyCode.Mouse2);
                        case ButtonPressTypes.PressUp:
                            return Input.GetKeyUp(KeyCode.Mouse2);
                    }
#else
                    switch (pressType)
                    {
                         case ButtonPressTypes.PressDown:
                            return VisionSDK.Instance.Input.GetButtonDown(GetControllerPeripheral(),ButtonType.Home);
                        case ButtonPressTypes.Press:
                            return VisionSDK.Instance.Input.GetButtonPressed(GetControllerPeripheral(), ButtonType.Home);
                        case ButtonPressTypes.PressUp:
                            return VisionSDK.Instance.Input.GetButtonUp(GetControllerPeripheral(), ButtonType.Home);
                    }
#endif
                    break;
            }
            return false;
        }              

        public float GetAix(ButtonType button)
        {
            if (peripheral == null)
            {
                peripheral = VisionSDK.Instance.Connections.GetPeripheral("XCobra-0");
            }
            ControllerInput input = GetControllerInput(peripheral);
            if (input == null)
                return 0;
            return input.GetAxis((int)button);
        }

        private static ControllerInput GetControllerInput(Peripheral peripheral)
        {
            ControllerInput input = peripheral.ControllerInput;

            if (peripheral is HmdPeripheral)
            {
                input = ((HmdPeripheral)peripheral).Input;
            }

            return input;
        }

        private Peripheral GetControllerPeripheral()
        {           
            return VisionSDK.Instance.Connections.GetPeripheral("XCobra-0");
        }
#endif
    }
}