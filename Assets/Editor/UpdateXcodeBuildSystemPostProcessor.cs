using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

#if UNITY_IOS || UNITY_TVOS
// Create specific aliases for iOS.Xcode imports.
// Unity Editor on macOS can report a conflict with other plugins
using PlistDocument = UnityEditor.iOS.Xcode.PlistDocument;
using PlistElementDict = UnityEditor.iOS.Xcode.PlistElementDict;
#endif


/// <summary>
/// Reference: http://www.kittehface.com/2021/09/fixing-invalid-frameworks-folder-in-ios.html
/// </summary>
// ReSharper disable once CheckNamespace
public class UpdateXcodeBuildSystemPostProcessor : Editor

{
#if UNITY_IOS
    [PostProcessBuild(9999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) return;
        
        ModifyFrameworks(path);
        UpdateXcodeBuildSystem(path);
    }
    
    /// <summary>
    /// https://forum.unity.com/threads/2019-3-validation-on-upload-to-store-gives-unityframework-framework-contains-disallowed-file.751112/#post-6959378
    /// </summary>
    /// <param name="path"></param>
    private static void ModifyFrameworks(string path)
    {
        
        Debug.LogFormat("[UpdateXcodeBuildSystemPostProcessor.ModifyFrameworks] Path: {0}", path);
        string projPath = PBXProject.GetPBXProjectPath(path);
           
        var project = new PBXProject();
        project.ReadFromFile(projPath);
 
        string mainTargetGuid = project.GetUnityMainTargetGuid();
           
        foreach (var targetGuid in new[] { mainTargetGuid, project.GetUnityFrameworkTargetGuid() })
        {
            project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
        }
           
        project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
 
        project.WriteToFile(projPath);
    
    }

    private static void UpdateXcodeBuildSystem(string projectPath)
    {
        Debug.LogFormat("[UpdateXcodeBuildSystemPostProcessor.UpdateXcodeBuildSystem] Path: {0}", projectPath);
        var workspaceSettingsPath = Path.Combine(projectPath,
            "Unity-iPhone.xcodeproj/project.xcworkspace/xcshareddata/" +
            "WorkspaceSettings.xcsettings");


        if (File.Exists(workspaceSettingsPath))
        {
            // Read the plist document, and find the root element
            var workspaceSettings = new PlistDocument();
            workspaceSettings.ReadFromFile(workspaceSettingsPath);
            var root = workspaceSettings.root;

            // Modify the document as necessary.
            var workspaceSettingsChanged = false;

            // Remove the BuildSystemType entry because it specifies the
            // legacy Xcode build system, which is deprecated
            if (root.values.ContainsKey("BuildSystemType"))

            {
                root.values.Remove("BuildSystemType");

                workspaceSettingsChanged = true;
            }

            // If actual changes to the document occurred, write the result
            // back to disk.
            if (workspaceSettingsChanged)
            {
                Debug.Log("[UpdateXcodeBuildSystem] Writing updated workspace settings to disk.");

                try
                {
                    workspaceSettings.WriteToFile(workspaceSettingsPath);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(string.Format("[UpdateXcodeBuildSystem] Exception occurred writing workspace settings to disk: \n{0}",
                        e.Message));
                    throw;
                }
            }
            else
            {
                Debug.Log("[UpdateXcodeBuildSystem] Workspace settings did not require modifications.");
            }
        }
        else
        {
            Debug.LogWarningFormat("[UpdateXcodeBuildSystem] Could not find workspace settings file [{0}]",
                workspaceSettingsPath);
        }

        // Get the path to the Xcode project
        var pbxProjectPath = PBXProject.GetPBXProjectPath(projectPath);
        var pbxProject = new PBXProject();

        // Open the Xcode project
        pbxProject.ReadFromFile(pbxProjectPath);

        // Get the UnityFramework target GUID
        var unityFrameworkTargetGuid = pbxProject.GetUnityFrameworkTargetGuid();

        const string swiftVersion = "5.0";
        // Modify the Swift version in the UnityFramework target to a
        // compatible string
        pbxProject.SetBuildProperty(unityFrameworkTargetGuid,
            "SWIFT_VERSION", swiftVersion);

        // Write out the Xcode project
        pbxProject.WriteToFile(pbxProjectPath);

        Debug.LogFormat("[UpdateXcodeBuildSystem] updated Swift version in Xcode  project to {0}", swiftVersion);

    }
#endif
}