using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

namespace mParticle {
    public class BuildPostProcessor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
                // Get project into C#
                var projectPath = PBXProject.GetPBXProjectPath(path);
                var project = new PBXProject();
                project.ReadFromFile(projectPath);

                // Fix Xcode build settings
                var projectGUID = project.ProjectGuid();
                project.SetBuildProperty(projectGUID, "VALIDATE_WORKSPACE", "YES");
                project.SetBuildProperty(projectGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

                // Embed mParticle framework
                var mainTargetGUID = project.GetUnityMainTargetGuid();
                var frameworkFileGUID = project.FindFileGuidByProjectPath("Frameworks/Plugins/iOS/mParticle_Apple_SDK.framework");
                project.AddFileToEmbedFrameworks(mainTargetGUID, frameworkFileGUID);

                // Overwrite
                project.WriteToFile(projectPath);
            }
        }
    }
}