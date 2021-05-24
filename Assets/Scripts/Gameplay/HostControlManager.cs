using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Gameplay
{
    public class HostControlManager : MonoBehaviour
    {
        public Button enableContentPanel_Button;
        public Button disableContentPanel_Button;

        public GameObject contentPanel;

        void Awake()
        {
            enableContentPanel_Button.onClick.AddListener(EnableContentPanel);
            disableContentPanel_Button.onClick.AddListener(DisableContentPanel);
        }

        public void EnableContentPanel()
        {
            contentPanel.SetActive(true);
            enableContentPanel_Button.gameObject.SetActive(false);
        }

        public void DisableContentPanel()
        {
            contentPanel.SetActive(false);
            enableContentPanel_Button.gameObject.SetActive(true);
        }
    }
}