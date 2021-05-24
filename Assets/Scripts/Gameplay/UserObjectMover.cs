using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Gameplay
{
    public class UserObjectMover : MonoBehaviourPun
    {
        private GameObject _ARCamera;

        private void Awake()
        {
            _ARCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        void Update()
        {
            MoveObject();
        }

        private void MoveObject()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (base.photonView.IsMine)
            {
                gameObject.transform.position = _ARCamera.transform.position;
            }
#endif
        }
    }
}