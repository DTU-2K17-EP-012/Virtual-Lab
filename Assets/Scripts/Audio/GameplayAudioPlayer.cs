using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Audio
{
    public class GameplayAudioPlayer : MonoBehaviour
    {
        public AudioSource pieceMoveAS;
        public AudioSource pieceCaptureAS;
        public AudioSource buttonClickAS;
        public AudioSource dieRollAS;
        public AudioSource celebrationAS;
        public AudioSource pieceFinishAS;

        public void PlayPieceMove()
        {
            pieceMoveAS.Play();
        }
        public void PlayPieceCapture()
        {
            pieceCaptureAS.Play();
        }
        public void PlayButtonClick()
        {
            buttonClickAS.Play();
        }
        public void PlayDieRoll()
        {
            dieRollAS.Play();
        }
        public void PlayCelebration()
        {
            celebrationAS.Play();
        }
        public void PlayPieceFinish()
        {
            pieceFinishAS.Play();
        }
    }
}

