using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VirtualLab.Gameplay.UI
{
    public class ContentObjectUIElement : MonoBehaviour
    {
        [SerializeField]
        private Toggle moduleStateToggle;
        public Toggle ModuleStateToggle { get { return moduleStateToggle; } }

        [SerializeField]
        private TMP_Text moduleNameText;
        public TMP_Text ModuleNameText { get { return moduleNameText; } }
    }
}