using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VirtualLab.Gameplay.UI
{
    public class GameplayPlayerInfoManager : MonoBehaviour
    {
        public GameplayPlayerInfo[] playerInfos;
        public UnityEngine.UI.Button leaveGameButton;
        public GameObject leaveGamePanel;
        public GameObject roomClosingAdditionalMessage;
        public UnityEngine.UI.Button trueLeaveGameButton;
        public UnityEngine.UI.Button cancelLeaveGameButton;
        public TMPro.TextMeshProUGUI roomCodeText;
        private bool initialized = false;

        private void Awake()
        {
            leaveGamePanel.SetActive(false);
            roomClosingAdditionalMessage.SetActive(false);
            if (GameManager.Instance.AreGameplayRoomParametersReady)
            {
                RoomInit();
                initialized = true;
            }
        }
        private void RoomInit()
        {
            List<ColorType> activeColorTypes = GameManager.Instance.GetRoomActiveColorTypes();
            for (int i = 0; i < playerInfos.Length; i++)
            {
                bool activeInfo = activeColorTypes.Contains(playerInfos[i].colorType);
                playerInfos[i].Initialize(activeInfo);
                if (activeInfo)
                {
                    Player player = GameManager.Instance.GetPlayerByColorType(playerInfos[i].colorType);
                    playerInfos[i].SetInfo(player.NickName, !player.IsInactive);
                }
            }
            SetAllTurnIndicators(false);
            roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        }
        private void OnEnable()
        {
            GameManager.OnGameplayTurnStart += OnTurnStarted;
            GameManager.OnPlayerJoined += OnPlayerJoined;
            GameManager.OnPlayerLeft += OnPlayerLeft;
            leaveGameButton.onClick.AddListener(OnLeaveButtonClick);
            trueLeaveGameButton.onClick.AddListener(OnTrueLeaveButtonClick);
            cancelLeaveGameButton.onClick.AddListener(OnCancelLeaveButtonClick);
        }
        private void OnDisable()
        {
            GameManager.OnGameplayTurnStart -= OnTurnStarted;
            GameManager.OnPlayerJoined -= OnPlayerJoined;
            GameManager.OnPlayerLeft -= OnPlayerLeft;
            leaveGameButton.onClick.RemoveListener(OnLeaveButtonClick);
            trueLeaveGameButton.onClick.RemoveListener(OnTrueLeaveButtonClick);
            cancelLeaveGameButton.onClick.RemoveListener(OnCancelLeaveButtonClick);

        }
        private void Update()
        {
            if (!initialized)
            {
                if (GameManager.Instance.AreGameplayRoomParametersReady)
                {
                    initialized = true;
                    RoomInit();
                }
            }
        }
        private void OnTurnStarted(ColorType colorTurn)
        {
            for (int i = 0; i < playerInfos.Length; i++)
            {
                playerInfos[i].SetTurnStatus(playerInfos[i].colorType == colorTurn);
            }
        }
        private void OnPlayerJoined(Player player)
        {
            ColorType playerColor = GameManager.Instance.GetPlayersColorType(player);
            for (int i = 0; i < playerInfos.Length; i++)
            {
                if (playerInfos[i].colorType == playerColor)
                {
                    playerInfos[i].SetConnection(true);
                    break;
                }
            }
        }
        private void OnPlayerLeft(Player player)
        {
            ColorType playerColor = GameManager.Instance.GetPlayersColorType(player);
            for (int i = 0; i < playerInfos.Length; i++)
            {
                if (playerInfos[i].colorType == playerColor)
                {
                    playerInfos[i].SetConnection(false);
                    break;
                }
            }
        }
        private void SetAllTurnIndicators(bool turnStatus)
        {
            for (int i = 0; i < playerInfos.Length; i++)
            {
                playerInfos[i].SetTurnStatus(turnStatus);
            }
        }
        public void OnLeaveButtonClick()
        {
            leaveGamePanel.SetActive(true);
            if (GameManager.Instance.GetActivePlayerCount() == 1)
            {
                roomClosingAdditionalMessage.SetActive(true);
            }
            else
            {
                roomClosingAdditionalMessage.SetActive(false);
            }
        }
        public void OnTrueLeaveButtonClick()
        {
            PhotonNetwork.LeaveRoom();
        }
        public void OnCancelLeaveButtonClick()
        {
            leaveGamePanel.SetActive(false);
        }
    }
}

