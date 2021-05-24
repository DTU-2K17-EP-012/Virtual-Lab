using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioSource audioSource;
        public static MusicPlayer Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                //PlayMusic();
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
        public void PlayMusic()
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }
    }
}

