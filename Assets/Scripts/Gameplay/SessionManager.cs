using Photon.Pun;
using VirtualLab.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Gameplay
{
    public class SessionManager : MonoBehaviour
    {
        [Header("User Objects")]
        public GameObject playerPrefabR;
        public GameObject playerPrefabG;
        public GameObject playerPrefabB;
        public GameObject playerPrefabY;
        public GameObject roomOriginObject;

        [Header("UI Element Objects")]
        public GameObject contentPanelObject;
        public GameObject notePanelObject;
        public Button closeNotePanelButton;

        [Header("Demo Content Collection")]
        public List<GameObject> contentObjects;

        private GameObject[] userObjects;
        private Dictionary<string, GameObject> contentPrefabCollection;

        private GameObject localPlayerObject;
        private ColorType playerColor;

        private static SessionManager instance;
        public static SessionManager Instance
        {
            get => instance;
            private set => instance = value;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            AssignReferences();
        }

        private void Start()
        {
            AssignHostControls();
        }

        private void OnEnable()
        {
            PlayerSpecifier();


            Vector3 position = new Vector3(roomOriginObject.transform.position.x, roomOriginObject.transform.position.y, roomOriginObject.transform.position.z);

            MasterManager.NetworkInstantiate(localPlayerObject, position, Quaternion.identity);

            userObjects = GameObject.FindGameObjectsWithTag("User");

            GameManager.OnMasterClientChanged += AssignHostControls;
        }

        private void OnDisable()
        {
            GameManager.OnMasterClientChanged -= AssignHostControls;
        }

        private void AssignReferences()
        {
            contentPrefabCollection = new Dictionary<string, GameObject>();

            closeNotePanelButton.onClick.AddListener(OnCloseNotePanelClick);

            for (int i = 0; i < contentObjects.Count; i++)
            {
                ContentObjectController cocVal = contentObjects[i].GetComponent<ContentObjectController>();
                var idVal = cocVal.ContentId;
                contentPrefabCollection[idVal] = cocVal.AssemblyObject;
                cocVal.AssemblyObject.SetActive(false);
            }
        }

        private void PlayerSpecifier()
        {
            playerColor = GameManager.Instance.GetPlayersColorType(PhotonNetwork.LocalPlayer);

            if (playerColor == ColorType.Red)
                localPlayerObject = playerPrefabR;
            else if (playerColor == ColorType.Green)
                localPlayerObject = playerPrefabG;
            else if (playerColor == ColorType.Blue)
                localPlayerObject = playerPrefabB;
            else if (playerColor == ColorType.Yellow)
                localPlayerObject = playerPrefabY;
        }

        public void OnCloseNotePanelClick()
        {
            notePanelObject.SetActive(false);
        }

        public void AssignHostControls()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                contentPanelObject.SetActive(true);
            }
            else
            {
                contentPanelObject.SetActive(false);
            }
        }

        public void ChangeObjectState(string objectId, bool isActive)
        {

            PhotonView pv = GetComponent<PhotonView>();
            pv.RPC("RPC_ChangeObjectState", RpcTarget.AllBuffered, objectId, isActive);
            Debug.Log("ChangeState RPC called successfully.");
        }

        #region Photon RPC
        [PunRPC]
        void RPC_ChangeObjectState(string objectId, bool isActive)
        {
            contentPrefabCollection[objectId].SetActive(isActive);
        }

        #endregion
    }
}
