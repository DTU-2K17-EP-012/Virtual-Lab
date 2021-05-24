using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Content.UI
{
    public class TransformController : MonoBehaviour
    {
        public Slider xPositionSlider;
        public Slider xRotattionSlider;

        void Update()
        {
            Vector3 positionVec = gameObject.transform.localPosition;
            positionVec.x = xPositionSlider.value;
            gameObject.transform.localPosition = positionVec;

            Vector3 rotationVec = gameObject.transform.localRotation.eulerAngles;
            rotationVec.y = xRotattionSlider.value;
            gameObject.transform.localRotation = Quaternion.Euler(rotationVec);
        }
    }
}