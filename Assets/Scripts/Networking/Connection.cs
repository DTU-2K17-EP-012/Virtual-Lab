using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VirtualLab.Data;
using ExitGames.Client.Photon;
using System;
using VirtualLab.Gameplay;

namespace VirtualLab.Networking
{
    public class Connection : MonoBehaviourPunCallbacks
    {
        //public GameManager gameManagerPrefab;
        private const byte CT_PIECESTATE_CODE = 10;
        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonPeer.RegisterType(typeof(PieceState), CT_PIECESTATE_CODE, PieceState.Serialize, PieceState.Deserialize);
            string userID = "";
            if (!SaveData.IsUserIDSet)
            {
                userID = GenerateNewUserID();
                SaveData.PUNUserID = userID;
            }
            else
            {
                userID = SaveData.PUNUserID;
            }
            Debug.Log(userID);
            if (PhotonNetwork.AuthValues == null)
            {
                PhotonNetwork.AuthValues = new AuthenticationValues(userID);
            }
            else
            {
                PhotonNetwork.AuthValues.UserId = userID;
            }
            PhotonNetwork.NickName = Data.SaveData.NickName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                if (!PhotonNetwork.InLobby && PhotonNetwork.IsConnectedAndReady)
                {
                    PhotonNetwork.JoinLobby();
                }
            }
            /*if (GameManager.Instance == null)
            {
                GameObject.Instantiate(gameManagerPrefab);
            }*/

        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server");
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);
            /*if (Data.SaveData.PlayerInRoom)
            {
                PhotonNetwork.RejoinRoom(Data.SaveData.LastRoomCode);
            }*/
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected for reason:({cause})");
            PhotonNetwork.ConnectUsingSettings();
        }

        /*private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                PhotonNetwork.Disconnect();
            }
        }
        private void Update()
        {
            if (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState == PeerStateValue.Disconnected)
            {
                if (!PhotonNetwork.ReconnectAndRejoin())
                {
                    Debug.Log("Failed reconnecting and joining!!", this);
                }
                else
                {
                    Debug.Log("Successful reconnected and joined!", this);
                }
            }
        }*/
        private string GenerateNewUserID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

