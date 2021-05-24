using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using WTUtils;

namespace VirtualLab.Gameplay.UI
{
    public class RoomInfoPanel : MonoBehaviourPunCallbacks
    {
        public Audio.MainMenuSFXPlayer sfxPlayer;
        [Header("Room Code")]
        public GameObject roomCodePanelRoot;
        public TMPro.TextMeshProUGUI roomCodeText;
        [Header("Player Count")]
        public GameObject playerCountRoot;
        public TMPro.TextMeshProUGUI playerCountText;
        public Color notEnoughPlayersTextColor, enoughPlayersTextColor;

        [Header("Player Infos")]
        public GameObject playersRoot;
        public PlayerInfoUI[] playerInfos;
        [Header("Start Game Button")]
        public GameObject startGameButtonRoot;
        public UnityEngine.UI.Button startGameButton;
        private bool initialized = false;
        private void Awake()
        {
            if (GameManager.Instance != null && GameManager.Instance.AreRoomParametersReady)
            {
                RoomInit();
                initialized = true;
            }
        }
        private void RoomInit()
        {
            //SetPlayerInfoPanelVisible(PhotonNetwork.InRoom);
            DeactivateAllPlayerInfos();
            startGameButton.onClick.AddListener(OnStartGameClicked);
            Debug.Log($"RoomType: {GameManager.Instance.RoomType}");
            roomCodePanelRoot.SetActive(GameManager.Instance.RoomType == GameManager.ROOMTYPE_PRIVATE);

            GameManager.OnGameStateChanged += UpdateGameReadyButton;
            GameManager.OnPlayerInfosChanged += UpdatePlayerInfos;
            PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayerInfos;
            //DeactivateAllPlayerInfos();
        }
        public override void OnEnable()
        {
            base.OnEnable();

            //DeactivateAllPlayerInfos();
        }
        public override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnGameStateChanged -= UpdateGameReadyButton;
            GameManager.OnPlayerInfosChanged -= UpdatePlayerInfos;
            PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayerInfos;
        }
        private void Update()
        {
            if (!initialized && GameManager.Instance.AreRoomParametersReady)
            {
                RoomInit();
                initialized = true;
            }

        }
        #region PUN_Callbacks
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            //SetPlayerInfoPanelVisible(true);
            /*DeactivateAllPlayerInfos();
            UpdatePlayerInfos();*/
            roomCodeText.text = PhotonNetwork.CurrentRoom.Name;

        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            initialized = false;
            //SetPlayerInfoPanelVisible(false);
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            //playerInfos[otherPlayer.ActorNumber].SetPlayerActive(false);
            //UpdatePlayerCount();
            //UpdatePlayerInfos();

            //UpdatePlayerInfos();
            int playerNumber = otherPlayer.GetPlayerNumber();
            if (playerNumber.IsBetween(-1, 4))
            {
                playerInfos[otherPlayer.GetPlayerNumber()].SetPlayerActive(false);
                playerInfos[otherPlayer.GetPlayerNumber()].Show(false);
            }
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            UpdateGameReadyButton();
        }
        #endregion
        #region UI_Callbacks
        private void OnStartGameClicked()
        {
            if (!GameManager.Instance.CanLocalPlayerStartGame())
            {
                return;
            }
            GameManager.Instance.StartGame();
            sfxPlayer.PlayButtonClick();
        }
        #endregion
        public void DeactivateAllPlayerInfos()
        {
            for (int i = 0; i < playerInfos.Length; i++)
            {
                playerInfos[i].SetPlayerActive(false);
                playerInfos[i].Show(false);
            }
        }
        public void UpdatePlayerInfos()
        {
            bool[] playersActive = new bool[] { false, false, false, false };
            if (PlayerNumbering.SortedPlayers != null)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
                {
                    if (i < PlayerNumbering.SortedPlayers.Length && PlayerNumbering.SortedPlayers[i] != null)
                    {
                        Player player = PlayerNumbering.SortedPlayers[i];
                        int playerNumber = player.GetPlayerNumber();
                        if (playerNumber.IsBetween(-1, 4))
                        {
                            playersActive[playerNumber] = !player.IsInactive;
                            playerInfos[playerNumber].SetInfo(player.NickName, GameManager.Instance.GetPlayerColor(player), playerNumber);
                            playerInfos[playerNumber].SetPlayerActive(!player.IsInactive);
                            playerInfos[playerNumber].Show(!player.IsInactive);
                        }

                    }
                }
            }

            for (int i = 0; i < playersActive.Length; i++)
            {
                if (!playersActive[i])
                {
                    playerInfos[i].SetPlayerActive(false);
                }
            }
            UpdatePlayerCount();
        }
        private void UpdatePlayerCount()
        {
            int numberOfPlayers = GameManager.Instance.GetPlayerCount();
            playerCountText.text = $"{numberOfPlayers}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
            playerCountText.color = (GameManager.Instance.RoomHasEnoughPlayers()) ? enoughPlayersTextColor : notEnoughPlayersTextColor;
            UpdateGameReadyButton();
        }
        public void UpdateGameReadyButton()
        {
            startGameButtonRoot.gameObject.SetActive(PhotonNetwork.IsMasterClient && !GameManager.Instance.GameStarted);
            startGameButton.interactable = GameManager.Instance.CanLocalPlayerStartGame();
        }
        private void SetPlayerInfoPanelVisible(bool visible)
        {
            playerCountRoot.SetActive(visible);
            playersRoot.SetActive(visible);
            roomCodePanelRoot.SetActive(visible);
            UpdateGameReadyButton();
        }

    }
}

