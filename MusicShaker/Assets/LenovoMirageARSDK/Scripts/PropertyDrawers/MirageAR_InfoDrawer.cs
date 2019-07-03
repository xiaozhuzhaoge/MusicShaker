using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif  // UNITY_EDITOR

/// Use to display an Info box in the inspector for a Monobehaviour or ScriptableObject.
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class MirageAR_Info : PropertyAttribute
{
    public string text;
    public int numLines;

    public MirageAR_Info(string text, int numLines)
    {
        this.text = text;
        this.numLines = numLines;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MirageAR_Info))]
public class MirageAR_InfoDrawer : DecoratorDrawer
{
    MirageAR_Info info
    {
        get { return ((MirageAR_Info)attribute); }
    }

    public override float GetHeight()
    {
        return GetHeightForLines(info.numLines);
    }

    public override void OnGUI(Rect position)
    {
        Draw(position, info.text);
    }

    public static float GetHeightForLines(int numLines)
    {
        return EditorGUIUtility.singleLineHeight * numLines;
    }

    public static void Draw(Rect position, string text)
    {
        position.height -= EditorGUIUtility.standardVerticalSpacing;

        int oldFontSize = EditorStyles.helpBox.fontSize;
        EditorStyles.helpBox.fontSize = 11;
        FontStyle oldFontStyle = EditorStyles.helpBox.fontStyle;
        EditorStyles.helpBox.fontStyle = FontStyle.Bold;
        bool oldWordWrap = EditorStyles.helpBox.wordWrap;
        EditorStyles.helpBox.wordWrap = false;

        EditorGUI.HelpBox(position, text, MessageType.Info);

        EditorStyles.helpBox.fontSize = oldFontSize;
        EditorStyles.helpBox.fontStyle = oldFontStyle;
        EditorStyles.helpBox.wordWrap = oldWordWrap;

    }
}

#endif  // UNITY_EDITOR
