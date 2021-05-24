using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using VirtualLab.Data;
using RUIS.UI;
using VirtualLab.Gameplay;

namespace VirtualLab.UI
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        public Audio.MainMenuSFXPlayer sfxPlayer;
        public SingleNotification notification;
        public InputField playerNameField;
        public InputField roomNameField;
        public Button confirmNameButton;
        public Button joinRoomButton;
        public Button createRoomButton;
        public Button quickConnectButton;
        public Button joinRoomBackButton;
        public Button roomBackButton;
        public Button quitGameButton;
        public Button quitConfirmationButton;
        public Button closeQuitConfirmationButton;
        public Button tutorialButton;
        public Button closeTutorialButton;
        public Button creditsButton;
        public Button closeCreditsButton;

        public UI_System uiSystem;
        public UI_Screen roomScreen;
        public UI_Screen mainMenuScreen;
        public UI_Screen usernameScreen;

        public GameObject tutorialPanel;
        public GameObject quitConfirmationPanel;
        public GameObject creditsPanel;
        public GameObject roomCodePanel;

        //Room Settings
        private const int roomNameLength = 4;

        private string playerName = "";
        private string roomName = "";
        public List<RoomInfo> roomList = new List<RoomInfo>();
        private List<string> playerNames = new List<string>();
        private bool tryToRejoin = false;
        private string lastRejoinRoom = "";
        private void Awake()
        {
            playerNameField.text = Data.SaveData.NickName;
            playerName = Data.SaveData.NickName;
        }
        public override void OnEnable()
        {
            base.OnEnable();
            //Register Buttons
            joinRoomButton.onClick.AddListener(OnClickJoinRoom);
            createRoomButton.onClick.AddListener(OnClickCreateRoom);
            quickConnectButton.onClick.AddListener(OnClickQuickConnect);
            joinRoomBackButton.onClick.AddListener(OnClickBackFromJoin);
            roomBackButton.onClick.AddListener(OnClickLeaveRoom);
            quitGameButton.onClick.AddListener(OnClickQuitGame);
            quitConfirmationButton.onClick.AddListener(onClickQuitConfirmation);
            closeQuitConfirmationButton.onClick.AddListener(onClickCloseQuitConfirmation);
            tutorialButton.onClick.AddListener(onClickTutorial);
            closeTutorialButton.onClick.AddListener(onClickCloseTutorial);
            creditsButton.onClick.AddListener(onClickCredits);
            closeCreditsButton.onClick.AddListener(onClickCloseCredits);
            confirmNameButton.onClick.AddListener(OnClickConfirmName);


            //Register Input
            playerNameField.onValueChanged.AddListener(OnChangedPlayerName);
            roomNameField.onValueChanged.AddListener(OnChangedRoomName);
            //Game Manager
            GameManager.OnUnstartedGameJoined += OnJoinedRoomUnstarted;
            GameManager.OnLocalPlayerLeft += OnLocalPlayerLeftRoom;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            //Register Buttons
            joinRoomButton.onClick.RemoveListener(OnClickJoinRoom);
            createRoomButton.onClick.RemoveListener(OnClickCreateRoom);
            quickConnectButton.onClick.RemoveListener(OnClickQuickConnect);
            joinRoomBackButton.onClick.RemoveListener(OnClickBackFromJoin);
            roomBackButton.onClick.RemoveListener(OnClickLeaveRoom);
            quitGameButton.onClick.RemoveListener(OnClickQuitGame);
            confirmNameButton.onClick.RemoveListener(OnClickConfirmName);

            //Register Input
            playerNameField.onValueChanged.RemoveListener(OnChangedPlayerName);
            roomNameField.onValueChanged.RemoveListener(OnChangedRoomName);

            //Game Manager
            GameManager.OnUnstartedGameJoined -= OnJoinedRoomUnstarted;
            GameManager.OnLocalPlayerLeft -= OnLocalPlayerLeftRoom;

        }
        private void Update()
        {
            bool baseButtonCondition = PhotonNetwork.IsConnected && PhotonNetwork.InLobby && !string.IsNullOrEmpty(playerName);
            confirmNameButton.interactable = !string.IsNullOrEmpty(playerName);
            joinRoomButton.interactable = baseButtonCondition && !string.IsNullOrEmpty(roomName) && roomName.Length == roomNameLength;
            createRoomButton.interactable = baseButtonCondition;
            quickConnectButton.interactable = baseButtonCondition;
        }

        //UI Events
        public void OnClickCreateRoom()
        {
            sfxPlayer.PlayButtonClick();
            if (!PhotonNetwork.IsConnected)
            {
                notification.CreateNotification("Client not connected while trying to create a room");
            }

            string roomCode = GetRandomRoomCode();
            Debug.Log("Conencting to:" + roomCode);
            PhotonNetwork.CreateRoom(roomCode, MasterManager.GameSettings.DefaultPrivateRoomOptions);
        }
        public void OnClickJoinRoom()
        {
            sfxPlayer.PlayButtonClick();
            if (!PhotonNetwork.IsConnected)
            {
                notification.CreateNotification("Client not connected while trying to join a room");
            }
            /*if (!RoomExists(roomName))
            {
                notification.CreateNotification($"Room {roomName} doesn't exist");
            }*/
            PhotonNetwork.JoinRoom(roomName);
            Debug.Log($"Joining room {roomName}");
        }
        public void OnClickQuickConnect()
        {
            sfxPlayer.PlayButtonClick();
            if (!PhotonNetwork.IsConnected)
            {
                notification.CreateNotification("Client not connected while trying to join a random room");
            }
            //Check if player is currently in a unstarted public room
            /*foreach (RoomInfo room in roomList)
            {
                if (!room.RemovedFromList && room.IsOpen && ((int)room.CustomProperties[GameManager.ROOMKEY_ROOMTYPE]) == GameManager.ROOMTYPE_PUBLIC)
                {
                    Photon
                }
            }*/
            PhotonNetwork.JoinRandomRoom();
        }
        public void OnClickConfirmName()
        {
            sfxPlayer.PlayButtonClick();
            uiSystem.SwitchScreens(mainMenuScreen);
        }
        public void onClickQuitConfirmation()
        {
            sfxPlayer.PlayButtonClick();
            quitConfirmationPanel.SetActive(true);
        }
        public void onClickCloseQuitConfirmation()
        {
            sfxPlayer.PlayButtonClick();
            quitConfirmationPanel.SetActive(false);
        }
        public void onClickTutorial()
        {
            sfxPlayer.PlayButtonClick();
            tutorialPanel.SetActive(true);
        }
        public void onClickCloseTutorial()
        {
            sfxPlayer.PlayButtonClick();
            tutorialPanel.SetActive(false);
        }
        public void onClickCredits()
        {
            sfxPlayer.PlayButtonClick();
            creditsPanel.SetActive(true);
        }
        public void onClickCloseCredits()
        {
            sfxPlayer.PlayButtonClick();
            creditsPanel.SetActive(false);
        }
        public void OnClickQuitGame()
        {
            sfxPlayer.PlayButtonClick();
            Application.Quit();
        }

        public void OnChangedPlayerName(string newName)
        {
            playerName = newName;
            confirmNameButton.interactable = !string.IsNullOrEmpty(playerName);
            PhotonNetwork.NickName = playerName;
            Data.SaveData.NickName = playerName;
        }
        public void OnChangedRoomName(string newName)
        {
            if (newName != newName.ToUpper())
            {
                roomNameField.SetTextWithoutNotify(newName.ToUpper());
                newName = newName.ToUpper();
            }
            roomName = newName;
        }
        public void OnClickBackFromJoin()
        {
            sfxPlayer.PlayButtonClick();
            uiSystem.SwitchScreens(mainMenuScreen);
        }
        public void OnClickLeaveRoom()
        {
            sfxPlayer.PlayButtonClick();
            PhotonNetwork.Disconnect();
        }
        public void OnSplashScreenFinished()
        {
            if (!RuntimeInfo.Instance.LoginPrompted)
            {
                uiSystem.SwitchScreens(usernameScreen);
                RuntimeInfo.Instance.LoginPrompted = true;
            }
            else
            {
                uiSystem.SwitchScreens(mainMenuScreen);
            }
        }
        //PUN Callbacks
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log($"Lobby joined. Trying to rejoin:{tryToRejoin}");
            if (tryToRejoin)
            {
                PhotonNetwork.RejoinRoom(roomName);
                Debug.Log($"Trying to rejoin {roomName}");
                tryToRejoin = false;
                lastRejoinRoom = roomName;
            }
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            this.roomList = roomList;

        }
        public override void OnCreatedRoom()
        {
            notification.CreateNotification("Room created successfully");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            notification.CreateNotification($"Room Creation Failed. Reason: {message}");
        }
        public override void OnJoinedRoom()
        {
            //notification.CreateNotification("Room joined successfully");
            /*if (PhotonNetwork.CurrentRoom != null)
            {
                Debug.Log(PhotonNetwork.CurrentRoom.Name);
            }*/
            //uiSystem.SwitchScreens(roomScreen);
            //SetMenuVisible(false);
            //PhotonNetwork.LocalPlayer.Actor
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Room joining failed. Reason: {returnCode} {message}");
            PhotonNetwork.JoinLobby();
            if (lastRejoinRoom == roomName)
            {
                //Rejoining failed, returning
                notification.CreateNotification($"Room joining failed. Reason: {message}");
                return;
            }

            if (returnCode == ErrorCode.GameClosed || returnCode == ErrorCode.GameDoesNotExist || returnCode == ErrorCode.JoinFailedFoundInactiveJoiner || returnCode == ErrorCode.GameFull)
            {
                tryToRejoin = true;
            }
            else
            {
                notification.CreateNotification($"Room joining failed. Reason: {message}");
            }
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //notification.CreateNotification($"Quick Connect failed. Reason: {message}");
            PhotonNetwork.CreateRoom(GetRandomRoomCode(), MasterManager.GameSettings.DefaultPublicRoomOptions, TypedLobby.Default);
        }
        public void OnLocalPlayerLeftRoom()
        {
            //base.OnLeftRoom();
            Debug.Log("Room left");
            uiSystem.SwitchScreens(mainMenuScreen);
            //SetMenuVisible(true);
        }

        private string GetRandomRoomCode()
        {
            System.Random random = new System.Random();
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            string code = new string(System.Linq.Enumerable.Repeat(chars, roomNameLength).Select(s => s[random.Next(s.Length)]).ToArray());
            while (RoomExists(code))
            {
                code = new string(System.Linq.Enumerable.Repeat(chars, roomNameLength).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return code;
            //RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        }

        private bool RoomExists(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                if (roomList != null)
                {
                    for (int i = 0; i < roomList.Count; i++)
                    {
                        if (!roomList[i].RemovedFromList && roomList[i].Name == roomName)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void OnJoinedRoomUnstarted()
        {
            uiSystem.SwitchScreens(roomScreen);
            //roomCodePanel.SetActive(GameManager.Instance.RoomType == GameManager.ROOMTYPE_PRIVATE);
        }
        /*private void SetMenuVisible(bool visible)
        {
            createMenuPanel.SetActive(visible);
            menuBackground.SetActive(visible);
        }*/
    }
}

