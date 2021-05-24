using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using VirtualLab.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VirtualLab.Data
{
    [CreateAssetMenu(menuName = "Data/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager>
    {
        public GameSettings gameSettings;
        public VisualSettings visualSettings;
        public static GameSettings GameSettings { get { return Instance.gameSettings; } }
        public static VisualSettings VisualSettings { get { return Instance.visualSettings; } }

        #region Network Instantiate

        [SerializeField]
        private List<NetworkedPrefab> _networkedPrefabs = new List<NetworkedPrefab>();

        // this method instantiates the prefab over the network from Resources folder through its respective path
        public static GameObject NetworkInstantiate(GameObject obj, Vector3 position, Quaternion rotation)
        {
            // traverse the _networkedPrefabs list to locate the particular prefab called passed as argument
            foreach (NetworkedPrefab networkedPrefab in Instance._networkedPrefabs)
            {
                // checking if the prefab matches or not for each item of list
                if (networkedPrefab.Prefab == obj)
                {
                    // safe check
                    if (networkedPrefab.Path != string.Empty)
                    {
                        // instantiate if it matches and return it
                        GameObject result = PhotonNetwork.Instantiate(networkedPrefab.Path, position, rotation);
                        return result;
                    }
                    else
                    {
                        Debug.LogError("Path is empty for gameObject name: " + networkedPrefab.Prefab.name);
                    }
                }
            }

            return null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void PopulateNetworkedPrefabs()
        {
            // This code needs to be run atleast once through the editor by playing in editor
            // for objects to serialize in the list before trying it on a Build
            // otherwise we can use an editor script to call this function before Build
#if UNITY_EDITOR
            // clearing out the _networkedPrefabs as it keeps concatenating the list because of Singleton behaviour
            Instance._networkedPrefabs.Clear();

            GameObject[] results = Resources.LoadAll<GameObject>("");

            // traversing the entire results array which contains all the GameObjects in Resources Folder
            for (int i = 0; i < results.Length; i++)
            {
                // only adding those objects to _networkedPrefabs that have a PhotonView component
                if (results[i].GetComponent<PhotonView>() != null)
                {
                    string path = AssetDatabase.GetAssetPath(results[i]);
                    Instance._networkedPrefabs.Add(new NetworkedPrefab(results[i], path));
                }
            }

            // Debug purpose
            for (int i = 0; i < Instance._networkedPrefabs.Count; i++)
            {
                Debug.Log(Instance._networkedPrefabs[i].Prefab.name + " [" + Instance._networkedPrefabs[i].Path + "]");
            }
#endif
        }

        #endregion
    }
}

