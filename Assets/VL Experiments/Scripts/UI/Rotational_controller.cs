using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Content.UI
{
    public class Rotational_controller : MonoBehaviour
    {
        public Slider y_slider;
        public Slider x_slider;

        public GameObject obj2rotate;


        private void Update()
        {
            var rotationVector = obj2rotate.transform.localEulerAngles;
            rotationVector.y = y_slider.value;
            rotationVector.x = x_slider.value;
            obj2rotate.transform.localRotation = Quaternion.Euler(rotationVector);

        }
    }
}