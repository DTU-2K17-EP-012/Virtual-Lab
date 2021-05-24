using Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VirtualLab.Gameplay
{
    public class UserObjectController : MonoBehaviour
    {
        public enum UserType { Red, Green, Blue, Yellow };

        public UserType user;
        public bool isMeshVisible = true;
        public Canvas userInfoCanvas;
        public TMP_Text usernameText;
        public TMP_Text positionText;
        public GameObject meshObject;

        private void Awake()
        {
            SetUserInfo();
            if (GetComponent<PhotonView>().IsMine)
            {
                SwitchVisibility(false);
            }

        }

        void Update()
        {
            positionText.text = transform.position.ToString();
        }

        private void SwitchVisibility(bool val)
        {
            MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();

            meshRenderer.enabled = val;

            userInfoCanvas.gameObject.SetActive(false);

            isMeshVisible = val;
        }

        private void SetUserInfo()
        {
            usernameText.text = GetComponent<PhotonView>().Controller.NickName.ToString();
        }
    }
}