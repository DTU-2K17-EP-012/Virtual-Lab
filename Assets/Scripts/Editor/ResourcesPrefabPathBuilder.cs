#if UNITY_EDITOR
using VirtualLab.Data;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class ResourcesPrefabPathBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        // running the fucntion through editor script prior to build everytime to populate
        MasterManager.PopulateNetworkedPrefabs();
    }
}
#endif