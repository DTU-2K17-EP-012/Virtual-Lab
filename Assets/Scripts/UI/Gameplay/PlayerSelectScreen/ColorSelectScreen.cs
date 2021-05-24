using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace VirtualLab.Gameplay.UI
{
    public class ColorSelectScreen : MonoBehaviour
    {
        public ColorOption[] colorOptions;
        public TMPro.TextMeshProUGUI messageText;
        public PlayerSelectionManager playerSelectionManager;
        public string otherPlayerPickingSuffix;
        public string localPlayerPickingMessage;
        private Player playerCurrentlyPicking;
        private bool initialized = false;

        private void Awake()
        {
            if (GameManager.Instance.AreRoomParametersReady)
            {
                initialized = true;
                RoomInit();
            }
        }
        private void RoomInit()
        {
            //Show(false);
            PrepareAndShow();
            Dictionary<ColorType, Player> colorPlayerDict = GameManager.Instance.GetAllPlayersColorTypes();
            foreach (KeyValuePair<ColorType, Player> kv in colorPlayerDict)
            {
                if (kv.Value != null)
                {
                    PlayerPickedColor(kv.Value, kv.Key);
                }
            }
        }
        private void Update()
        {
            if (!initialized)
            {
                if (GameManager.Instance.AreRoomParametersReady)
                {
                    initialized = true;
                    RoomInit();
                }
            }
        }
        private void OnEnable()
        {
            GameManager.OnPlayerPickedColor += PlayerPickedColor;
            GameManager.OnNewPlayerShouldPickColor += SetNewPlayer;
        }
        private void OnDisable()
        {
            GameManager.OnPlayerPickedColor -= PlayerPickedColor;
            GameManager.OnNewPlayerShouldPickColor -= SetNewPlayer;
        }
        public void Reset()
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                //colorOptions[i].SetShownColor(Data.MasterManager.VisualSettings.GetColorForColorType(colorOptions[i].colorType));
                colorOptions[i].Reset();
            }
        }
        public void PrepareAndShow()
        {
            Reset();
            SetNewPlayer();
        }
        private void ChangeMessage()
        {
            if (GameManager.Instance.ColorsPicked)
            {
                return;
            }
            if (playerCurrentlyPicking.IsLocal)
            {
                messageText.text = localPlayerPickingMessage;
            }
            else
            {
                messageText.text = $"{playerCurrentlyPicking.NickName} {otherPlayerPickingSuffix}";
            }
        }
        public void SetNewPlayer()
        {
            Player plr = null;
            if (GameManager.Instance.GetNextPlayerWithUnpickedColor(out plr))
            {
                playerCurrentlyPicking = plr;
                ChangeMessage();
                playerSelectionManager.SetSelectionActive(playerCurrentlyPicking.IsLocal);
            }
            else
            {
                playerSelectionManager.SetSelectionActive(false);
            }
        }
        public void ColorSelected(ColorType colorType)
        {
            GameManager.Instance.SetPlayerColorType(PhotonNetwork.LocalPlayer, colorType);
        }
        public void PlayerPickedColor(Player player, ColorType color)
        {
            GetColorOptionForColorType(color).PickColor(player.NickName);
        }
        private ColorOption GetColorOptionForColorType(ColorType color)
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                if (colorOptions[i].colorType == color)
                {
                    return colorOptions[i];
                }
            }
            throw new System.Exception($"There is no ColorOption for PlayerColor {color.ToString()}");
        }
        public bool IsLocalPlayerPicking()
        {
            return !GameManager.Instance.ColorsPicked && playerCurrentlyPicking.IsLocal;
        }
    }
}

