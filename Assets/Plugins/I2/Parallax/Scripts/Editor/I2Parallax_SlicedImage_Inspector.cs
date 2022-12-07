using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace I2.Parallax
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(I2Parallax_SlicedImage))]
    public class I2Parallax_SlicedImage_Inspector : Editor
    {
        SerializedProperty mProp_DepthIn, mProp_DepthOut;

        SerializedProperty mProp_Slice_Top, mProp_Slice_Bottom, mProp_Slice_Left, mProp_Slice_Right;
        SerializedProperty mProp_DepthFactor_TopLeft, mProp_DepthFactor_TopRight, mProp_DepthFactor_BottomLeft, mProp_DepthFactor_BottomRight;

        SerializedProperty mProp_Constrain_Top, mProp_Constrain_Bottom, mProp_Constrain_Left, mProp_Constrain_Right;
        SerializedProperty mProp_AutoExpand_Top, mProp_AutoExpand_Bottom, mProp_AutoExpand_Left, mProp_AutoExpand_Right;

        SerializedProperty mProp_Subdivisions;


        public void OnEnable()
        {
            mProp_DepthIn                 = serializedObject.FindProperty("_DepthIn");
            mProp_DepthOut                = serializedObject.FindProperty("_DepthOut");

            mProp_Slice_Top               = serializedObject.FindProperty("_Slice_Top");
            mProp_Slice_Bottom            = serializedObject.FindProperty("_Slice_Bottom");
            mProp_Slice_Left              = serializedObject.FindProperty("_Slice_Left");
            mProp_Slice_Right             = serializedObject.FindProperty("_Slice_Right");

            mProp_DepthFactor_TopLeft     = serializedObject.FindProperty("_DepthFactor_TopLeft");
            mProp_DepthFactor_TopRight    = serializedObject.FindProperty("_DepthFactor_TopRight");
            mProp_DepthFactor_BottomLeft  = serializedObject.FindProperty("_DepthFactor_BottomLeft");
            mProp_DepthFactor_BottomRight = serializedObject.FindProperty("_DepthFactor_BottomRight");

            mProp_Constrain_Top           = serializedObject.FindProperty("_Constrain_Top");
            mProp_Constrain_Bottom        = serializedObject.FindProperty("_Constrain_Bottom");
            mProp_Constrain_Left          = serializedObject.FindProperty("_Constrain_Left");
            mProp_Constrain_Right         = serializedObject.FindProperty("_Constrain_Right");

            mProp_AutoExpand_Top          = serializedObject.FindProperty("_AutoExpand_Top");
            mProp_AutoExpand_Bottom       = serializedObject.FindProperty("_AutoExpand_Bottom");
            mProp_AutoExpand_Left         = serializedObject.FindProperty("_AutoExpand_Left");
            mProp_AutoExpand_Right        = serializedObject.FindProperty("_AutoExpand_Right");

            mProp_Subdivisions            = serializedObject.FindProperty("_Subdivisions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfDirtyOrScript();

            var labelW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60;
            GUILayout.Space(15);

                OnGUI_Depth();
                OnGUI_Constraint();

                EditorGUIUtility.labelWidth = 80;
                EditorGUILayout.PropertyField(mProp_Subdivisions);
                EditorGUIUtility.labelWidth = labelW;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
                GUILayout.Label("Open Settings", GUILayout.Width(90));
                if (GUILayout.Button("I2 Parallax Settings", GUILayout.ExpandWidth(true)))
                    Selection.activeObject = I2ParallaxSettings.GetSettings();
            GUILayout.EndHorizontal();


            I2Parallax_Layer_Inspector.OnGUI_Footer("I2 Parallax", ParallaxUpgradeManager.GetVersion(), ParallaxUpgradeManager.HelpURL_forum, ParallaxUpgradeManager.HelpURL_Documentation);

            serializedObject.ApplyModifiedProperties();
        }

        private static void OnGUI_Header( string Text=null, string HelpURL=null)
        {
            GUI.backgroundColor = Color.Lerp(Color.black, Color.gray, 1);
            GUILayout.BeginVertical(ParallaxUpgradeManager.GUIStyle_Background, GUILayout.Height(1));
            GUI.backgroundColor = Color.white;

            if (!string.IsNullOrEmpty(Text))
            {
                if (GUILayout.Button(Text, ParallaxUpgradeManager.GUIStyle_Header))
                {
                    if (!string.IsNullOrEmpty(HelpURL))
                        Application.OpenURL(HelpURL);
                }
            }

            GUILayout.Space(10);
            GUILayout.BeginVertical("AS TextArea");
            GUILayout.Space(5);
        }

        void OnGUI_Constraint()
        {
            GUILayout.BeginHorizontal();
                GUILayout.Space(60);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Expand", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Constrain", EditorStyles.boldLabel, GUILayout.Width(70));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.Label("Top", GUILayout.Width(60));
                mProp_AutoExpand_Top.floatValue = EditorGUILayout.Slider(mProp_AutoExpand_Top.floatValue, 0, 1, GUILayout.ExpandWidth(true));
                GUILayout.Space(20);
                mProp_Constrain_Top.boolValue = EditorGUILayout.Toggle(mProp_Constrain_Top.boolValue, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.Label("Bottom", GUILayout.Width(60));
                mProp_AutoExpand_Bottom.floatValue = EditorGUILayout.Slider(mProp_AutoExpand_Bottom.floatValue, 0, 1, GUILayout.ExpandWidth(true));
                GUILayout.Space(20);
                mProp_Constrain_Bottom.boolValue = EditorGUILayout.Toggle(mProp_Constrain_Bottom.boolValue, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.Label("Left", GUILayout.Width(60));
                mProp_AutoExpand_Left.floatValue = EditorGUILayout.Slider(mProp_AutoExpand_Left.floatValue, 0, 1, GUILayout.ExpandWidth(true));
                GUILayout.Space(20);
                mProp_Constrain_Left.boolValue = EditorGUILayout.Toggle(mProp_Constrain_Left.boolValue, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.Label("Right", GUILayout.Width(60));
                mProp_AutoExpand_Right.floatValue = EditorGUILayout.Slider(mProp_AutoExpand_Right.floatValue, 0, 1, GUILayout.ExpandWidth(true));
                GUILayout.Space(20);
                mProp_Constrain_Right.boolValue = EditorGUILayout.Toggle(mProp_Constrain_Right.boolValue, GUILayout.Width(50));
            GUILayout.EndHorizontal();

        }
        

        void OnGUI_Depth()
        {
            GUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(mProp_DepthOut, new GUIContent("Outside"));
                EditorGUILayout.PropertyField(mProp_DepthIn, new GUIContent("Inside"));

                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(80));
                        GUILayout.Label("Depth Scale", EditorStyles.boldLabel);
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                        EditorGUIUtility.labelWidth = 110;    
                        EditorGUILayout.PropertyField(mProp_DepthFactor_TopLeft, new GUIContent("IN Top-Left"));
                        EditorGUILayout.PropertyField(mProp_DepthFactor_TopRight, new GUIContent("IN Top-Right"));
                        EditorGUILayout.PropertyField(mProp_DepthFactor_BottomLeft, new GUIContent("IN Bottom-Left"));
                        EditorGUILayout.PropertyField(mProp_DepthFactor_BottomRight, new GUIContent("IN Bottom-Right"));
                        EditorGUIUtility.labelWidth = 60;
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

            GUILayout.EndVertical();

            EditorGUILayout.PropertyField(mProp_Slice_Left, new GUIContent("Left"));
            EditorGUILayout.PropertyField(mProp_Slice_Right, new GUIContent("Right"));
            EditorGUILayout.PropertyField(mProp_Slice_Top, new GUIContent("Top"));
            EditorGUILayout.PropertyField(mProp_Slice_Bottom, new GUIContent("Bottom"));
            
            GUILayout.Space(10);
        }
    }
}