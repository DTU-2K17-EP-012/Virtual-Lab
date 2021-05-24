using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonBehaviour : MonoBehaviour
{
    //public GameObject button;
    public int inactiveDuration = 1;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();//Component gets the button from the Button object itself during the first frame
    }

    // Update is called once per frame
    void Update()
    {

        if (!(button.interactable))
        {
            //wait for 1seconds implementation
            StartCoroutine(ChangeState(1));
        }
    }

    IEnumerator ChangeState(int inactiveDuration)
    {
        //wait for 1 second and then set it button.interactable = true
        yield return new WaitForSeconds(inactiveDuration);
        button.interactable = true;
    }
}
