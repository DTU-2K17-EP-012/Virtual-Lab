using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Gameplay.UI
{
    public class GameplayPlayerInfo : MonoBehaviour
    {
        public ColorType colorType;
        public TMPro.TextMeshProUGUI playerNameField;
        public Image connectionImage;
        public Image turnIndicator;
        public Color connectedColor;
        public Color disconnectedColor;
        private bool playerIsPlaying;
        private bool playerIsConnected;
        public void Initialize(bool playing)
        {
            playerIsPlaying = playing;
            gameObject.SetActive(playing);
        }
        public void SetInfo(string playerName, bool isConnected)
        {
            playerNameField.text = playerName;
            SetConnection(isConnected);
        }
        public void SetConnection(bool isConnected)
        {
            playerIsConnected = isConnected;
            connectionImage.color = (playerIsConnected) ? connectedColor : disconnectedColor;
        }
        public void SetTurnStatus(bool isPlayersTurn)
        {
            turnIndicator.enabled = isPlayersTurn;
        }
    }
}

