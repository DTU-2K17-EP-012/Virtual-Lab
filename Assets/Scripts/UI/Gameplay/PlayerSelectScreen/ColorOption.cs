using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VirtualLab.Gameplay.UI
{
    public class ColorOption : MonoBehaviour
    {
        public ColorType colorType;
        public TMPro.TextMeshProUGUI playerNameField;
        public ColorSelectScreen colorPickPanel;

        public bool reserved = false;
        public void Reset()
        {
            reserved = false;
            playerNameField.text = "";
        }
        private void Update()
        {
            UpdateTextRotation();
        }
        public void PickColor(string playerName)
        {
            this.playerNameField.text = playerName;
            reserved = true;
        }
        private void UpdateTextRotation()
        {
            playerNameField.transform.rotation = Quaternion.identity;
        }
    }
}

