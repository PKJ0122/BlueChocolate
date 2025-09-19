using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerSettings.Android.bundleVersionCode++;

        // iOS의 Build Number를 1 증가시킵니다.
        int currentBuildNumber = 0;
        if (int.TryParse(PlayerSettings.iOS.buildNumber, out currentBuildNumber))
        {
            PlayerSettings.iOS.buildNumber = (currentBuildNumber + 1).ToString();
        }
    }
}