using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using VirtualLab.Gameplay.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    public VirtualLab.Audio.PlayerSelectionSFXPlayer sfxPlayer;
    public Transform playerSwitcherTransform;
    public int playerSelectionNumber;
    public ColorOption[] pieceGroupPrefabs;
    public VirtualLab.Gameplay.UI.ColorSelectScreen colorSelectScreen;
    private bool selectionActive = false;
    
    [Header("UI")]
    public GameObject UI_Selection;
    public GameObject UI_AfterSelection;
    public Button next_Button;
    public Button previous_Button;
    public Button select_Button;


    /*[Header("Scene Name(s)")]
    public string gameplaySceneName = "Scene_Gameplay";
    public string lobbySceneName = "Scene_Lobby";*/

    #region UNITY Methods

    // Start is called before the first frame update
    void Start()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);

        playerSelectionNumber = 0;
    }
    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        sfxPlayer.PlayChangeSelection();
        /*if (!selectionActive)
        {
            return;
        }*/
        playerSelectionNumber++;
        playerSelectionNumber %= 4;

        Debug.Log("Player Index: " + playerSelectionNumber);

        //buttons are disabled to avoid consecutive clicks
        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));
    }

    public void PreviousPlayer()
    {
        sfxPlayer.PlayChangeSelection();

        /*if (!selectionActive)
        {
            return;
        }*/
        playerSelectionNumber--;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = pieceGroupPrefabs.Length - 1;
        }

        Debug.Log("Player Index: " + playerSelectionNumber);

        //buttons are disabled to avoid consecutive clicks
        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
    }

    public void OnSelectButtonClicked()
    {
        /*UI_Selection.SetActive(false);
        UI_AfterSelection.SetActive(true);*/

        if (!selectionActive || pieceGroupPrefabs[playerSelectionNumber].reserved)
        {
            return;
        }
        colorSelectScreen.ColorSelected(pieceGroupPrefabs[playerSelectionNumber].colorType);
        sfxPlayer.PlayButtonClick();
        //Creating a new custom property through hashtable with string and int as key and value pairs.
        //var playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARSpinTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);

    }
    public void SetSelectionActive(bool active)
    {
        selectionActive = active;
        select_Button.interactable = selectionActive;
    }

    public void OnReselectButtonClicked()
    {
        //UI_Selection.SetActive(true);
        //UI_AfterSelection.SetActive(false);
    }

    /*public void OnStartButtonClicked()
    {
        SceneLoader.Instance.LoadScene(gameplaySceneName);
    }*/

    public void OnBackButtonClicked()
    {
        //SceneLoader.Instance.LoadScene(lobbySceneName);
        sfxPlayer.PlayButtonClick();
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;

        //rotate a vector by another vector
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            //This will rotate gameObject from initial value to final value by an amount set
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        /*Since the slerp will never reach the final roatation value but 
         * it will be very close to the final value after duration therefore 
         * on termination of loop we put the assign the final value to the 
         * rotation of the playerSwitcherTransform.
        */
        transformToRotate.rotation = finalRotation;

        //On completion of roatation vuttons are enabled again
        next_Button.enabled = true;
        previous_Button.enabled = true;
    }

    #endregion

}
