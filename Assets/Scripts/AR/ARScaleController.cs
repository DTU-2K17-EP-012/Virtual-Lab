using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

namespace VirtualLab.AR
{
    public class ARScaleController : MonoBehaviour
    {
        ARSessionOrigin m_ARSessionOrigin;
        public Slider scaleSlider;

        private void Awake()
        {
            m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
        }

        private void Start()
        {
            scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnSliderValueChanged(float value)
        {
            if (scaleSlider != null)
            {
                m_ARSessionOrigin.transform.localScale = Vector3.one / value;
            }
        }
    }
}