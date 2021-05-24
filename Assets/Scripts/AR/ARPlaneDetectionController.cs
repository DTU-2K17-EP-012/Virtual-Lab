using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

namespace VirtualLab.AR
{

    public class ARPlaneDetectionController : MonoBehaviour
    {
        ARPlaneManager m_ARPlaneManager;
        ARPlacementManager m_ARPlacementManager;

        public TextMeshProUGUI informUIPanel_Text;
        public GameObject UI_InformPanel_Gameobject;

        public Slider scaleSlider;
        public Button placeButton;
        public Button adjustButton;

        public bool keepScaleSlider = true;

        private void Awake()
        {
            m_ARPlacementManager = GetComponent<ARPlacementManager>();
            m_ARPlaneManager = GetComponent<ARPlaneManager>();

            placeButton.onClick.AddListener(DisableARPlacementAndPlaneDetection);
            adjustButton.onClick.AddListener(EnableARPlacementAndPlaneDetection);
        }

        private void Start()
        {
            placeButton.gameObject.SetActive(true);
            scaleSlider.gameObject.SetActive(keepScaleSlider);

            adjustButton.gameObject.SetActive(false);

            informUIPanel_Text.text = "Move the phone around to detect plane and place the Virtual-Object(s).";
        }

        private void SetPlaneActivation(bool value)
        {
            foreach (var plane in m_ARPlaneManager.trackables)
            {
                plane.gameObject.SetActive(value);
            }
        }

        public void DisableARPlacementAndPlaneDetection()
        {
            UI_InformPanel_Gameobject.SetActive(true);

            m_ARPlaneManager.enabled = false;
            m_ARPlacementManager.enabled = false;

            SetPlaneActivation(false);

            placeButton.gameObject.SetActive(false);
            adjustButton.gameObject.SetActive(true);

            scaleSlider.gameObject.SetActive(false);

            informUIPanel_Text.text = "Virtual-Object(s) placed.";

            StartCoroutine(DeactivateAfterSeconds(UI_InformPanel_Gameobject, 2f));
        }

        public void EnableARPlacementAndPlaneDetection()
        {
            UI_InformPanel_Gameobject.SetActive(true);

            m_ARPlaneManager.enabled = true;
            m_ARPlacementManager.enabled = true;

            SetPlaneActivation(true);

            placeButton.gameObject.SetActive(true);
            adjustButton.gameObject.SetActive(false);

            scaleSlider.gameObject.SetActive(keepScaleSlider);

            informUIPanel_Text.text = "Move the phone around to detect plane and place the Virtual-Object(s).";

            StartCoroutine(DeactivateAfterSeconds(UI_InformPanel_Gameobject, 2f));
        }

        IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
        {
            yield return new WaitForSeconds(_seconds);
            _gameObject.SetActive(false);
        }
    }
}
