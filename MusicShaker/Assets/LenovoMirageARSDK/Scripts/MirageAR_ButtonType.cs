using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ximmerse.InputSystem;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// The Define of the Button
    /// </summary>
    public enum ButtonType
    {
        // HMD Buttons
        HmdMenu = ControllerRawButton.DpadLeft,
        HmdBack = ControllerRawButton.DpadDown,
        HmdSelect = ControllerRawButton.DpadUp,

        // Saber Buttons
        SaberActivate = ControllerRawButton.LeftShoulder,
        SaberControl = ControllerRawButton.RightShoulder,

        // Universal Controller Buttons              
        Trigger = ControllerRawButton.LeftTrigger,
        App = ControllerRawButton.LeftShoulder,
        Home = ControllerRawButton.RightShoulder,
        Touchpad = TouchpadGestureType.DpadClick | TouchpadGestureType.DpadUp | TouchpadGestureType.DpadDown | TouchpadGestureType.DpadLeft | TouchpadGestureType.DpadRight,
    }

    /// <summary>
    /// The Define of the Touchpad Gesture
    /// </summary>
    public enum TouchpadGestureType
    {
        //Touchpad Touch
        TouchpadTouch = ControllerRawButton.LeftThumbMove,

        //Touchpad Swipe Gesture
        SwipeUp = ControllerRawButton.LeftThumbUp,
        SwipeDown = ControllerRawButton.LeftThumbDown,
        SwipeLeft = ControllerRawButton.LeftThumbLeft,
        SwipeRight = ControllerRawButton.LeftThumbRight,

        //Touchpad To Dpad
        DpadClick = ControllerRawButton.LeftThumb,
        DpadUp = ControllerRawButton.DpadUp,
        DpadDown = ControllerRawButton.DpadDown,
        DpadLeft = ControllerRawButton.DpadLeft,
        DpadRight = ControllerRawButton.DpadRight,
    }

}
