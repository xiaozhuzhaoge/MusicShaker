using UnityEditor;
using UnityEngine;

namespace LenovoMirageARSDK.Editor
{
    /// A custom editor for the MirageAR_EditorEmulator script.
    /// It adds an info panel describing how to controls.
    [CustomEditor(typeof(MirageAR_EditorEmulator)), CanEditMultipleObjects]
    public class MirageAR_EditorEmulatorEditor : UnityEditor.Editor
    {

        private float infoHeight;

        private const string INFO_TEXT = "Hmd Input Emulator :\n" +
                                         "   • Alt + Move Mouse = Change Hmd Yaw/Pitch\n" +
                                         "   • Ctrl + Movze Mouse = Change Hmd Roll\n"+
                                         "   • ASDWQE=Left,Right,Back,Forward,Down,Up Move\n"+
                                         "\n"+
                                         "Controller Input Emulator : Shift+\n"+
                                         "   • Move Mouse = Change Controller Orientation\n"+
                                         "   • Left Mouse Button = Touchpad Click\n"+
                                         "   • Right Mouse Button = App Click\n"+
                                         "   • Middle Mouse Button = Home Click\n";

        private const int NUM_INFO_LINES = 10;

        void OnEnable()
        {
            infoHeight = MirageAR_InfoDrawer.GetHeightForLines(NUM_INFO_LINES);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Rect rect = EditorGUILayout.GetControlRect(false, infoHeight);
            MirageAR_InfoDrawer.Draw(rect, INFO_TEXT);
        }
    }
}
