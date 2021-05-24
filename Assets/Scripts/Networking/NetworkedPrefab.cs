using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Networking
{
    [System.Serializable]
    public class NetworkedPrefab
    {
        public GameObject Prefab;
        public string Path;

        public NetworkedPrefab(GameObject obj, string path)
        {
            Prefab = obj;
            Path = ReturnModifiedPrefabPath(path);
        }

        // This method trims the "Assets" from the received path "Assets/Resources/File.prefab"
        // but we only need "File"
        private string ReturnModifiedPrefabPath(string path)
        {
            int extensionLength = System.IO.Path.GetExtension(path).Length;

            // length of "resources/"
            int additionalLength = 10;
            int startIndex = path.ToLower().IndexOf("resources") + additionalLength;

            if (startIndex == -1)
                return string.Empty;
            else
                return path.Substring(startIndex, path.Length - (startIndex + extensionLength));
        }
    }
}