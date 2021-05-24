using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualLab.Gameplay.UI;

namespace VirtualLab.Gameplay
{
    public class ContentObjectController : MonoBehaviour
    {
        [SerializeField]
        private string contentId;
        public string ContentId { get { return contentId; } }

        [SerializeField]
        private string moduleName;
        public string ModuleName { get { return moduleName; } }

        [SerializeField]
        private GameObject assemblyObject;
        public GameObject AssemblyObject { get { return assemblyObject; } }

        [SerializeField]
        private ContentObjectUIElement contentModuleUI;
        public ContentObjectUIElement ContentModuleUI { get { return contentModuleUI; } }

        private Toggle _moduleStateToggle;
        private TMP_Text _moduleNameText;

        private void Awake()
        {
            _moduleStateToggle = contentModuleUI.ModuleStateToggle;
            _moduleNameText = contentModuleUI.ModuleNameText;

            _moduleNameText.text = moduleName;
            _moduleStateToggle.onValueChanged.AddListener(delegate { ChangeState(_moduleStateToggle); });
        }

        void ChangeState(Toggle argToggle)
        {
            SessionManager.Instance.ChangeObjectState(ContentId, argToggle.isOn);
        }
    }
}