using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace I2.Parallax
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(I2Parallax_Layer))]
    public class I2Parallax_Layer_Inspector : Editor
    {
        SerializedProperty mProp_Depth, mProp_DepthFactor;

        SerializedProperty mProp_Constrain_Top, mProp_Constrain_Bottom, mProp_Constrain_Left, mProp_Constrain_Right;
        SerializedProperty mProp_AutoExpand_Top, mProp_AutoExpand_Bottom, mProp_AutoExpand_Left, mProp_AutoExpand_Right;

        public void OnEnable()
        {
            mProp_Depth             = serializedObject.FindProperty("_Depth");
            mProp_DepthFactor       = serializedObject.FindProperty("_DepthFactor");

            mProp_Constrain_Top     = serializedObject.FindProperty("_Constrain_Top");
            mProp_Constrain_Bottom  = serializedObject.FindProperty("_Constrain_Bottom");
            mProp_Constrain_Left    = serializedObject.FindProperty("_Constrain_Left");
            mProp_Constrain_Right   = serializedObject.FindProperty("_Constrain_Right");

            mProp_AutoExpand_Top    = serializedObject.FindProperty("_AutoExpand_Top");
            mProp_AutoExpand_Bottom = serializedObject.FindProperty("_AutoExpand_Bottom");
            mProp_AutoExpand_Left   = serializedObject.FindProperty("_AutoExpand_Left");
            mProp_AutoExpand_Right  = serializedObject.FindProperty("_AutoExpand_Right");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfDirtyOrScript();

            GUILayout.Space(15);

                OnGUI_Depth();
                OnGUI_Constraint();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
                GUILayout.Label("Open Settings", GUILayout.Width(90));
                if (GUILayout.Button("I2 Parallax Settings", GUILayout.ExpandWidth(true)))
                    Selection.activeObject = I2ParallaxSettings.GetSettings();
            GUILayout.EndHorizontal();


            OnGUI_Footer("I2 Parallax", ParallaxUpgradeManager.GetVersion(), ParallaxUpgradeManager.HelpURL_forum, ParallaxUpgradeManager.HelpURL_Documentation);


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

        public static void OnGUI_Footer(string pluginName, string pluginVersion, string helpURL, string documentationURL)
        {
            GUILayout.BeginHorizontal();
            string versionTip = "";

            GUILayout.Label(new GUIContent("v" + pluginVersion, versionTip), EditorStyles.miniLabel);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ask a Question", EditorStyles.miniLabel))
                Application.OpenURL(helpURL);

            GUILayout.Space(10);

            if (GUILayout.Button("Documentation", EditorStyles.miniLabel))
                Application.OpenURL(documentationURL);
            GUILayout.EndHorizontal();
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

            GUILayout.BeginHorizontal();
                GUILayout.Label("Depth", GUILayout.Width(60));
                EditorGUILayout.PropertyField(mProp_Depth, new GUIContent());
            GUILayout.EndHorizontal();


            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
                GUILayout.Space(60);
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("X", "Allow horizontal movement"), GUILayout.ExpandWidth(false));
                bool allowX = GUILayout.Toggle(mProp_DepthFactor.vector2Value.x > 0, new GUIContent("", "Allow horizontal movement"));

                GUILayout.Space(15);

                GUILayout.Label(new GUIContent("Y", "Allow vertical movement"), GUILayout.ExpandWidth(false));
                bool allowY = GUILayout.Toggle(mProp_DepthFactor.vector2Value.y > 0, new GUIContent("", "Allow vertical movement"));
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                mProp_DepthFactor.vector2Value = new Vector2(allowX?1:0, allowY?1:0);
            }

            GUILayout.EndVertical();

            GUILayout.Space(10);
        }
    }
}