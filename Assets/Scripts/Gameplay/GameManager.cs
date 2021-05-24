using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using VirtualLab.Gameplay.UI;
using UnityEngine;
using System;
using WTUtils;
using UnityEngine.SceneManagement;

namespace VirtualLab.Gameplay
{
    public enum ColorType { Blue, Red, Green, Yellow }
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get => instance;
            private set => instance = value;
        }
        public const int LOBBY_SCENE_NUMBER = 0;
        public const int LOADING_SCENE_ID = 1;
        public const int AVATARPICK_SCENE_NUMBER = 2;
        public const int GAMEPLAY_SCENE_NUMBER = 3;
        private const int MIN_PLAYERS = 2;
        
        //Custom Properties Keys
        public const string ROOMKEY_ROOMTYPE = "R0";
        public const string ROOMKEY_GAMESTARTED = "R1";
        public const string ROOMKEY_COLORSPICKED = "R2";
        public const string ROOMKEY_PLRCOLORIDS = "R3";
        public const string ROOMKEY_PLRCNTGS = "R4";
        public const string ROOMKEY_PLAYERTURN = "R5";
        public const string ROOMKEY_PIECESSTATE = "R6";
        public const string ROOMKEY_CAPTURESPERCOLOR = "R7";
        
        //Custom properties values
        public const int ROOMTYPE_PRIVATE = 0;
        public const int ROOMTYPE_PUBLIC = 1;
        
        //Events
        public static event Action OnGameStateChanged;
        public static event Action OnPlayerInfosChanged;
        public static event Action OnUnstartedGameJoined;
        public static event Action<Player, ColorType> OnPlayerPickedColor;
        public static event Action OnNewPlayerShouldPickColor;
        //public static event Action<Move> OnGameplayMoveMade;
        public static event Action<ColorType> OnGameplayMoveFailed;
        public static event Action<ColorType> OnGameplayTurnStart;
        public static event Action OnGameplayTurnInterrupted;
        //public static event Action<Dictionary<ColorType, Dictionary<PieceType, PieceState>>> OnGameplayInitializeBoardState;
        //public static event Action<DiceResult> OnDieRolled;
        public static event Action<Player> OnPlayerJoined;
        public static event Action<Player> OnPlayerLeft;
        public static event Action OnLocalPlayerLeft;
        public static event Action OnMasterClientChanged;

        private int sceneThatShouldBeLoaded = -1;
        private bool playerNumberingInitialized = false;
        
        public bool AreRoomParametersReady 
        { 
            get => playerNumberingInitialized; 
        }
        public bool AreGameplayRoomParametersReady 
        {
            get => AreRoomParametersReady;//&& GetRoomProperty<int>(ROOMKEY_PLAYERTURN) != -1; 
        }
        public bool GameStarted
        {
            get => GetRoomProperty<bool>(ROOMKEY_GAMESTARTED);
            private set => SetRoomProperty(ROOMKEY_GAMESTARTED, value);
        }
        public bool ColorsPicked
        {
            get => GetRoomProperty<bool>(ROOMKEY_COLORSPICKED);
            private set => SetRoomProperty(ROOMKEY_COLORSPICKED, value);
        }
        private int PlayerCountWhenGameStarted
        {
            get => GetRoomProperty<int>(ROOMKEY_PLRCNTGS);
            set => SetRoomProperty(ROOMKEY_PLRCNTGS, value);
        }
        private ColorType CurrentColorTurn
        {
            get => (ColorType)GetRoomProperty<int>(ROOMKEY_PLAYERTURN);
            set => SetRoomProperty(ROOMKEY_PLAYERTURN, (int)value);
        }
        public int RoomType => GetRoomProperty<int>(ROOMKEY_ROOMTYPE);
        public ColorType GetCurrentColorTurn() => CurrentColorTurn;
        
        #region Room_State_Methods
        
        public void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        public override void OnEnable()
        {
            base.OnEnable();
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }
        public void Reset()
        {
            /*if (IsOwnerOfTheRoom())
            {
                //colorPickPanel.Show(false);
            }*/
        }
        public void StartGame()
        {
            if (!CanLocalPlayerStartGame())
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            GameStarted = true;
            PlayerCountWhenGameStarted = GetPlayerCount();
            this.photonView.RPC("RPC_StartGame", RpcTarget.All);
            OnGameStateChanged?.Invoke();
        }
        public void StartGameplay()
        {
            if (CanLocalPlayerStartGame())
            {
                PhotonNetwork.LoadLevel(GAMEPLAY_SCENE_NUMBER);
            }
        }
        
        #endregion
        
        #region Room_Information
        
        private void SetRoomProperty(string key, object value)
        {
            Debug.Log($"Setting property ({key}:{value.ToString()})");
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
            roomProperties.Add(key, value);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
        private T GetRoomProperty<T>(string key)
        {
            return (T)PhotonNetwork.CurrentRoom.CustomProperties[key];
        }
        public int GetPlayerCount()
        {
            int count = 0;
            if (PlayerNumbering.SortedPlayers == null)
            {
                return count;
            }
            if (GameStarted)
            {
                return PlayerCountWhenGameStarted;
            }
            for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PlayerNumbering.SortedPlayers[i] != null && !PlayerNumbering.SortedPlayers[i].IsInactive)
                {
                    count++;
                }
            }
            return count;
            /*for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (GameStarted || !PlayerNumbering.SortedPlayers[i].IsInactive)
                {
                    count++;
                }
            }*/
            //return count;
        }
        public int GetActivePlayerCount()
        {
            int count = 0;
            if (PlayerNumbering.SortedPlayers != null)
            {
                for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
                {
                    Player plr = PlayerNumbering.SortedPlayers[i];
                    if (plr != null && !plr.IsInactive)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public bool RoomHasEnoughPlayers()
        {
            return GetPlayerCount() >= MIN_PLAYERS;
        }
        public bool IsOwnerOfTheRoom()
        {
            return PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient;
        }
        public bool CanLocalPlayerStartGame()
        {
            return IsOwnerOfTheRoom() && RoomHasEnoughPlayers();
        }

        #endregion

        #region Prep_PUN_RPCs
        
        [PunRPC]
        public void RPC_StartGame()
        {
            PhotonNetwork.LoadLevel(AVATARPICK_SCENE_NUMBER);
            //colorPickPanel.PrepareAndShow();
        }

        #endregion
        
        #region PUN_Callbacks
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            if (PhotonNetwork.IsMasterClient)
            {
                Reset();
            }
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Data.SaveData.PlayerInRoom = true;
            Data.SaveData.LastRoomCode = PhotonNetwork.CurrentRoom.Name;
            Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");
            
            //If rejoined, update game state for local player
            if (!GameStarted)
            {
                OnUnstartedGameJoined?.Invoke();
            }
            if (PhotonNetwork.LocalPlayer.HasRejoined)
            {
                OnPlayerInfosChanged?.Invoke();

                //If Game hasn't started
                if (!GameStarted)
                {
                }
                else if (!ColorsPicked)
                {
                    sceneThatShouldBeLoaded = AVATARPICK_SCENE_NUMBER;

                    if (playerNumberingInitialized)
                    {
                        
                    }
                }
                else
                {
                    sceneThatShouldBeLoaded = GAMEPLAY_SCENE_NUMBER;
                    
                    //Gameplay scene
                    if (playerNumberingInitialized && PlayerNumbering.SortedPlayers != null)
                    {
                    
                    }
                }
            }
        }
        private void OnPlayerNumberingChanged()
        {
            playerNumberingInitialized = PlayerNumbering.SortedPlayers != null;
            if (sceneThatShouldBeLoaded != -1)
            {
                //OnRoomParametersReady?.Invoke();
            }
            Debug.Log("Player numbering changed");
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            playerNumberingInitialized = false;
            sceneThatShouldBeLoaded = -1;
            Data.SaveData.PlayerInRoom = false;
            if (SceneManager.GetActiveScene().buildIndex == AVATARPICK_SCENE_NUMBER || SceneManager.GetActiveScene().buildIndex == GAMEPLAY_SCENE_NUMBER)
            {
                Destroy(this.gameObject);
                Instance = null;
                SceneManager.LoadScene(LOBBY_SCENE_NUMBER);
            }
            OnLocalPlayerLeft?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"{otherPlayer.NickName} left room");
            Debug.Log($"Local Player {(IsOwnerOfTheRoom() ? "is" : "isnt") } owner");
            base.OnPlayerLeftRoom(otherPlayer);
            if (IsOwnerOfTheRoom())// && !GameStarted)
            {
                //Destroying inactive players so new ones can join
                Debug.LogWarning($"{otherPlayer.NickName} left the room!");
                PhotonNetwork.DestroyPlayerObjects(otherPlayer);
            }
            if (GameStarted)
            {
                if (!ColorsPicked)
                {
                    OnNewPlayerShouldPickColor?.Invoke();
                }
                else
                {
                    if (IsOwnerOfTheRoom() && CurrentColorTurn == GetPlayersColorType(otherPlayer))
                    {
                        OnGameplayTurnInterrupted?.Invoke();
                        //SetTurnNextActivePlayer();
                    }
                }
            }
            OnPlayerLeft?.Invoke(otherPlayer);

        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            OnPlayerJoined?.Invoke(newPlayer);
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            Debug.Log($"{newMasterClient.NickName} is new master");
            OnMasterClientChanged?.Invoke();
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (propertiesThatChanged.ContainsKey(ROOMKEY_GAMESTARTED))
            {
                OnGameStateChanged?.Invoke();
                //roomInfoPanel.UpdateGameReadyButton();
            }
            if (propertiesThatChanged.ContainsKey(ROOMKEY_PLRCOLORIDS))
            {
                int playersWithColor = 0;
                playersWithColor = UpdateColorPicker(propertiesThatChanged);
            }
            if (propertiesThatChanged.ContainsKey(ROOMKEY_PLAYERTURN))
            {

                ColorType playerColorType = (ColorType)propertiesThatChanged[ROOMKEY_PLAYERTURN];
                Debug.Log($"Changed playerTurn to {playerColorType}");
                OnGameplayTurnStart?.Invoke(playerColorType);
            }
        }

        #endregion
        
        #region ColorPickerScreen
        public int UpdateColorPicker(ExitGames.Client.Photon.Hashtable properties)
        {
            int playersWithColor = 0;
            int[] colorPlayers = (int[])properties[ROOMKEY_PLRCOLORIDS];
            for (int i = 0; i < colorPlayers.Length; i++)
            {
                if (colorPlayers[i] != -1)
                {
                    ColorType cType = (ColorType)i;
                    Player player;
                    if (GetPlayerByNumber(colorPlayers[i], out player))
                    {
                        OnPlayerPickedColor?.Invoke(player, cType);
                        //colorPickPanel.PlayerPickedColor(player, cType);
                        playersWithColor++;
                    };

                }
            }
            //roomInfoPanel.UpdatePlayerInfos();
            if (playersWithColor < GetPlayerCount())
            {
                OnNewPlayerShouldPickColor?.Invoke();
                //colorPickPanel.SetNewPlayer();
            }
            else
            {
                if (CanLocalPlayerStartGame())
                {
                    ColorsPicked = true;
                }

                //Load gameplay scene
                StartGameplay();
                //colorPickPanel.Show(false);
            }
            return playersWithColor;
        }
        private bool IsPlayerColorSet(Player player)
        {
            int playerNumber = player.GetPlayerNumber();
            int[] colorPlayerNumbers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            for (int i = 0; i < colorPlayerNumbers.Length; i++)
            {
                if (colorPlayerNumbers[i] == playerNumber)
                {
                    return true;
                }
            }
            return false;
        }
        public Color GetPlayerColor(Player player)
        {
            if (IsPlayerColorSet(player))
            {
                return Data.MasterManager.VisualSettings.GetColorForColorType(GetPlayersColorType(player));
            }
            else
            {
                return Data.MasterManager.VisualSettings.unselectedColor;
            }
        }
        public bool GetNextPlayerWithUnpickedColor(out Player nextPlayer)
        {
            //Checking for active players
            for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PlayerNumbering.SortedPlayers[i] != null)
                {
                    Player player = PlayerNumbering.SortedPlayers[i];
                    if (!player.IsInactive && !IsPlayerColorSet(player))
                    {
                        nextPlayer = player;
                        return true;
                    }
                }
            }

            //If there aren't any active left, pick random colors for inactive
            if (IsOwnerOfTheRoom())
            {
                FinishColorPick();
            }
            nextPlayer = null;
            return false;
        }
        private void FinishColorPick()
        {
            int[] colorPlayers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            bool shouldWrite = false;
            for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PlayerNumbering.SortedPlayers[i] != null)
                {
                    Player player = PlayerNumbering.SortedPlayers[i];
                    if (!IsPlayerColorSet(player))
                    {
                        for (int c = 0; c < 4; c++)
                        {
                            ColorType colorType = (ColorType)c;
                            if (colorPlayers[c] == -1)
                            {
                                colorPlayers[c] = player.GetPlayerNumber();
                                //SetPlayerColorType(player, colorType);
                                shouldWrite = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (shouldWrite)
            {
                SetRoomProperty(ROOMKEY_PLRCOLORIDS, colorPlayers);
            }

        }
        public void SetPlayerColorType(Player player, ColorType colorType)
        {
            int[] colorPlayers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            for (int i = 0; i < colorPlayers.Length; i++)
            {
                if (i == (int)colorType)
                {
                    colorPlayers[i] = player.GetPlayerNumber();
                    break;
                }
            }
            SetRoomProperty(ROOMKEY_PLRCOLORIDS, colorPlayers);
        }

        #endregion
        
        #region Player Information
        private bool GetPlayerByNumber(int playerNumber, out Player player)
        {
            Player[] players = PlayerNumbering.SortedPlayers;
            for (int i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PlayerNumbering.SortedPlayers[i] != null && PlayerNumbering.SortedPlayers[i].GetPlayerNumber() == playerNumber)
                {
                    player = PlayerNumbering.SortedPlayers[i];
                    return true;
                }
            }
            player = null;
            return false;
        }
        public Dictionary<ColorType, Player> GetAllPlayersColorTypes()
        {
            int[] colorPlayerNumbers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            Dictionary<ColorType, Player> colorPlayerDict = new Dictionary<ColorType, Player>();
            for (int i = 0; i < colorPlayerNumbers.Length; i++)
            {
                ColorType colorType = (ColorType)i;
                if (colorPlayerNumbers[i] == -1)
                {
                    colorPlayerDict.Add(colorType, null);
                }
                else
                {
                    Player plr;
                    if (GetPlayerByNumber(colorPlayerNumbers[i], out plr))
                    {
                        colorPlayerDict.Add(colorType, plr);
                    }
                    else
                    {
                        throw new System.Exception($"ColorType {colorType.ToString()} is set but player for number {colorPlayerNumbers[i]} can't be found!");
                    }
                }
            }
            return colorPlayerDict;
        }

        public Player GetPlayerByColorType(ColorType colorType)
        {
            int[] colorPlayerNumbers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            for (int i = 0; i < colorPlayerNumbers.Length; i++)
            {
                if ((ColorType)i == colorType && colorPlayerNumbers[i] != -1)
                {
                    Player player;
                    if (GetPlayerByNumber(colorPlayerNumbers[i], out player))
                    {
                        return player;
                    }
                    else
                    {
                        //Player probably deleted
                        return null;
                        //throw new System.Exception($"ColorType {colorType.ToString()} is set but player for number {colorPlayerNumbers[i]} can't be found!");
                    }
                }
            }
            throw new System.Exception($"Player for ColorType {colorType.ToString()} isn't set!");
        }
        public ColorType GetPlayersColorType(Player player)
        {
            int playerNumber = player.GetPlayerNumber();
            int[] colorPlayerNumbers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            for (int i = 0; i < colorPlayerNumbers.Length; i++)
            {
                if (colorPlayerNumbers[i] == playerNumber)
                {
                    return (ColorType)i;
                }
            }
            throw new System.Exception($"ColorType for Player {player.NickName} isn't set!");
        }
        public ColorType LocalPlayerColorType()
        {
            return GetPlayersColorType(PhotonNetwork.LocalPlayer);
        }

        #endregion
        
        #region Gameplay

        public List<ColorType> GetRoomActiveColorTypes()
        {
            int[] colorPlayerNumbers = GetRoomProperty<int[]>(ROOMKEY_PLRCOLORIDS);
            List<ColorType> activeColorTypes = new List<ColorType>();
            for (int i = 0; i < colorPlayerNumbers.Length; i++)
            {
                if (colorPlayerNumbers[i] != -1)
                {
                    activeColorTypes.Add((ColorType)i);
                }
            }
            return activeColorTypes;
        }
        
        #endregion
        
    }
}