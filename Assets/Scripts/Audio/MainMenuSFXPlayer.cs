using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Audio
{
    public class MainMenuSFXPlayer : MonoBehaviour
    {
        public AudioSource buttonClickAS;

        public void PlayButtonClick()
        {
            buttonClickAS.Play();
        }
    }
}

