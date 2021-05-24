using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Audio
{
    public class PlayerSelectionSFXPlayer : MonoBehaviour
    {
        public AudioSource changeSelectionAS;
        public AudioSource buttonClickAS;
        public void PlayChangeSelection()
        {
            changeSelectionAS.Play();
        }
        public void PlayButtonClick()
        {
            buttonClickAS.Play();
        }
    }
}

