using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class SignInWithApplePostprocessor : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        string projPath = PBXProject.GetPBXProjectPath(path);
        // changed
        ProjectCapabilityManager projCapability = new ProjectCapabilityManager(projPath, "Unity-iPhone/losingcatsway.entitlements", "Unity-iPhone");
        projCapability.AddSignInWithApple();
        projCapability.WriteToFile();
    }
}
