using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using VirtualLab.Gameplay;
using UnityEngine;

namespace VirtualLab.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        private string gameVersion = "0.0.0";
        public string GameVersion { get => gameVersion; }
        [SerializeField]
        private string nickName = "worntunic";
        //public string Nickname { get => nickname; }
        public string NickName
        {
            get
            {
                int value = Random.Range(0, 9999);
                return nickName + value;
            }
        }
        static ExitGames.Client.Photon.Hashtable DefaultPrivateRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { GameManager.ROOMKEY_ROOMTYPE, GameManager.ROOMTYPE_PRIVATE},
                { GameManager.ROOMKEY_GAMESTARTED, false },
                { GameManager.ROOMKEY_COLORSPICKED, false },
                { GameManager.ROOMKEY_PLRCOLORIDS, new int[] { -1, -1, -1, -1 } },
                { GameManager.ROOMKEY_PLAYERTURN, -1 },
                //{ GameManager.ROOMKEY_PIECESSTATE, new PieceState[] {
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Queen),
                //}},
                { GameManager.ROOMKEY_CAPTURESPERCOLOR, new byte[] {0, 0, 0, 0}}

            };
        static ExitGames.Client.Photon.Hashtable DefaultPublicRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { GameManager.ROOMKEY_ROOMTYPE, GameManager.ROOMTYPE_PUBLIC},
                { GameManager.ROOMKEY_GAMESTARTED, false },
                { GameManager.ROOMKEY_COLORSPICKED, false },
                { GameManager.ROOMKEY_PLRCOLORIDS, new int[] { -1, -1, -1, -1 } },
                { GameManager.ROOMKEY_PLAYERTURN, -1 },
                //{ GameManager.ROOMKEY_PIECESSTATE, new PieceState[] {
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Blue, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Red, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Green, PieceType.Queen),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Rook),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Bishop),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Knight),
                //    PieceState.GetDefaultState(ColorType.Yellow, PieceType.Queen),
                //}},
                { GameManager.ROOMKEY_CAPTURESPERCOLOR, new byte[] {0, 0, 0, 0}}
            };
        private RoomOptions defaultPrivateRoomOptions = new RoomOptions()
        {
            MaxPlayers = 4,
            PlayerTtl = 1000 * 60 * 15, //15 minutes
            EmptyRoomTtl = 0, //0 minutes
            IsVisible = false,
            CustomRoomProperties = DefaultPrivateRoomProperties
        };
        private RoomOptions defaultPublicRoomOptions = new RoomOptions()
        {
            MaxPlayers = 4,
            PlayerTtl = 1000 * 60 * 15, //15 minutes
            EmptyRoomTtl = 0, //0 minutes
            IsVisible = true,
            CustomRoomProperties = DefaultPublicRoomProperties
        };
        public RoomOptions DefaultPrivateRoomOptions => defaultPrivateRoomOptions;
        public RoomOptions DefaultPublicRoomOptions => defaultPublicRoomOptions;
    }
}
