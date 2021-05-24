using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Gameplay.UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        public Color oddPlayerBackgroundColor, evenPlayerBackgroundColor;
        public Color textColor;

        public TMPro.TextMeshProUGUI nameText;
        public UnityEngine.UI.Image colorImage;
        public UnityEngine.UI.Image backgroundImage;

        private string nickName;
        private Color pieceColor;
        private int playerNumber;

        private void Awake()
        {
            gameObject.SetActive(false);
            //colorImage.SetActive(false);
        }

        public void SetInfo(string nickName, Color pieceColor, int playerNumber)
        {
            this.nickName = nickName;
            this.pieceColor = pieceColor;
            nameText.text = this.nickName;
            //colorImage.color = this.pieceColor;
            backgroundImage.color = (playerNumber % 2 == 0) ? evenPlayerBackgroundColor : oddPlayerBackgroundColor;
        }

        public void Show(bool show)
        {
            this.gameObject.SetActive(show);
        }

        public void SetPlayerActive(bool playerActive)
        {
            //colorImage.color = (!playerActive) ? Color.grey : pieceColor;
            nameText.color = (!playerActive) ? Color.grey : textColor;
        }
    }
}

