using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Debugging
{
    public class VersionNumber : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI versionText;

        private void Awake()
        {
            if (Debug.isDebugBuild || Application.isEditor)
            {
                gameObject.SetActive(true);
                SetVersionText();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void SetVersionText()
        {
            versionText.text = $"v{Application.version}";
        }
    }
}

