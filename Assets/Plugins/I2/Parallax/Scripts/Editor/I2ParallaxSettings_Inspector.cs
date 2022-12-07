using UnityEngine;
using UnityEditor;
using System.Collections;

namespace I2.Parallax
{
	[CustomEditor(typeof(I2ParallaxSettings))]
	public class PerspParallaxSettings_Inspector : Editor
    {
        SerializedProperty mSerialized_UseGyro, mSerialized_UseMouse,
							mSerialized_Controller_Gyro, mSerialized_Controller_Accel, mSerialized_Controller_Mouse;
        SerializedProperty mProp_Gyro_AutoCenter, mProp_Gyro_AutoRecenter_MinMovement, mProp_Gyro_AutoRecenter_Delay, mProp_Gyro_AutoRecenter_Damping;
        SerializedProperty mProp_Accel_AutoCenter, mProp_Accel_AutoRecenter_MinMovement, mProp_Accel_AutoRecenter_Delay, mProp_Accel_AutoRecenter_Damping, mProp_Accel_Sensitivity;
        SerializedProperty mProp_Mouse_Sensitivity;

        public virtual void OnEnable()
        {
            mSerialized_UseGyro = serializedObject.FindProperty("_UseGyro");
            mSerialized_UseMouse = serializedObject.FindProperty("_UseMouse");

            mSerialized_Controller_Gyro = serializedObject.FindProperty("_Controller_Gyro");
            mSerialized_Controller_Accel = serializedObject.FindProperty("_Controller_Accelerometer");
            mSerialized_Controller_Mouse = serializedObject.FindProperty("_Controller_Mouse");

            mProp_Gyro_AutoCenter                = mSerialized_Controller_Gyro.FindPropertyRelative("_AutoCenter");
            mProp_Gyro_AutoRecenter_MinMovement  = mSerialized_Controller_Gyro.FindPropertyRelative("_AutoRecenter_MinMovement");
            mProp_Gyro_AutoRecenter_Delay        = mSerialized_Controller_Gyro.FindPropertyRelative("_AutoRecenter_Delay");
            mProp_Gyro_AutoRecenter_Damping      = mSerialized_Controller_Gyro.FindPropertyRelative("_AutoRecenter_Damping");

            mProp_Accel_Sensitivity              = mSerialized_Controller_Accel.FindPropertyRelative("_Sensitivity");
            mProp_Accel_AutoCenter               = mSerialized_Controller_Accel.FindPropertyRelative("_AutoCenter");
            mProp_Accel_AutoRecenter_MinMovement = mSerialized_Controller_Accel.FindPropertyRelative("_AutoRecenter_MinMovement");
            mProp_Accel_AutoRecenter_Delay       = mSerialized_Controller_Accel.FindPropertyRelative("_AutoRecenter_Delay");
            mProp_Accel_AutoRecenter_Damping     = mSerialized_Controller_Accel.FindPropertyRelative("_AutoRecenter_Damping");

            mProp_Mouse_Sensitivity              = mSerialized_Controller_Mouse.FindPropertyRelative("_Sensitivity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfDirtyOrScript();

            //--[ Header ]------------------------------
            {
                GUI.backgroundColor = Color.Lerp(Color.black, Color.gray, 1);
                GUILayout.BeginVertical(ParallaxUpgradeManager.GUIStyle_Background, GUILayout.Height(1));
                GUI.backgroundColor = Color.white;

                //if (GUILayout.Button("Layer", ParallaxUpgradeManager.GUIStyle_Header))
                //{
                //Application.OpenURL(ParallaxUpgradeManager.HelpURL_Documentation);
                //}

                GUILayout.Space(10);
            }

            OnGUI_Content();
            EditorGUIUtility.labelWidth = 50;

            //--[ Footer ]------------------------------

            I2Parallax_Layer_Inspector.OnGUI_Footer("I2 Parallax", ParallaxUpgradeManager.GetVersion(), ParallaxUpgradeManager.HelpURL_forum, ParallaxUpgradeManager.HelpURL_Documentation);
            GUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }

        void OnGUI_Content()
        {
            EditorGUIUtility.labelWidth = 90;
            GUILayout.BeginHorizontal("Box");
                EditorGUILayout.PropertyField(mSerialized_UseGyro);
                GUILayout.FlexibleSpace();
                EditorGUILayout.PropertyField(mSerialized_UseMouse);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);


            GUI.enabled = mSerialized_UseMouse.boolValue;
            GUILayout.Label("Mouse", EditorStyles.boldLabel);
            GUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(mProp_Mouse_Sensitivity, new GUIContent("Sensitivity", "When the mouse is moved the rotation is scaled by this amount"));
            GUILayout.EndVertical();

            GUI.enabled = true;
            GUILayout.Space(10);


            GUI.enabled = mSerialized_UseGyro.boolValue;
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Gyroscope", EditorStyles.boldLabel);
            GUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(mProp_Accel_Sensitivity, new GUIContent("Sensitivity", "When gyroscope is not available, the accelerometer is used to find the view direction, this scales the movement speed"));
                GUILayout.Space(5);
                EditorGUILayout.PropertyField( mProp_Gyro_AutoCenter              , new GUIContent("Auto-Recenter", "When selected, the view will softly adjust to match the viewer rotation"));

                GUI.enabled = mProp_Gyro_AutoCenter.boolValue;
                EditorGUILayout.PropertyField( mProp_Gyro_AutoRecenter_MinMovement, new GUIContent("Min Move", "If the device moves less than this amount, it is considered 'not moving' and after the Delay, it will start recentering"));
                EditorGUILayout.PropertyField( mProp_Gyro_AutoRecenter_Delay      , new GUIContent("Delay", "Once the device is not moving, it will wait this many seconds to start recentering"));
                EditorGUILayout.PropertyField( mProp_Gyro_AutoRecenter_Damping    , new GUIContent("Damping", "How fast move while re-centering"));
            GUILayout.EndVertical();
            GUI.enabled = true;

            if (EditorGUI.EndChangeCheck())
            {
                mProp_Accel_AutoCenter.boolValue                = mProp_Gyro_AutoCenter.boolValue;
                mProp_Accel_AutoRecenter_MinMovement.floatValue = mProp_Gyro_AutoRecenter_MinMovement.floatValue;
                mProp_Accel_AutoRecenter_Delay.floatValue       = mProp_Gyro_AutoRecenter_Delay.floatValue;
                mProp_Accel_AutoRecenter_Damping.floatValue     = mProp_Gyro_AutoRecenter_Damping.floatValue;
            }
        }
	}
}