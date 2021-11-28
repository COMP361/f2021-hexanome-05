using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{

    public TMPro.TMP_Text infoText;
    public Button createSessionButton;

    private Client thisClient;
    
    void Start()
    {
        //Create Client object and get the button.
        thisClient = Client.Instance();
        createSessionButton = this.gameObject.GetComponent<Button>();

        //Add onclick listener to button for "login" function.
        createSessionButton.onClick.AddListener(createSessionAttempt);

        thisClient.CreateSuccessEvent += createSuccessResult;
        thisClient.CreateFailureEvent += createFailure;
    }

    private void createSessionAttempt(){
        thisClient.createSession();
    }

    private void createFailure(string inputError){
        infoText.text = inputError;
    }

    private void createSuccessResult(string result){
        return;
    }


}
