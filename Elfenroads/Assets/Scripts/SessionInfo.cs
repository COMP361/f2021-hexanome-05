using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionInfo : MonoBehaviour
{
    private Client thisClient;
    private bool isElfenland = true;
    private string Variant;

    void Start(){
        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    public void setClient(){
        thisClient = Client.Instance();
        Debug.Log("SessionInfo object ID is now " + thisClient.thisSessionID);
    }

    public Client getClient(){
        return thisClient;
    }

    public string getSessionID(){
        return thisClient.thisSessionID;
    }

    public void setIsElfenLand(bool input){
        isElfenland = input;
        Debug.Log("isElfenLand set to: " + input);
    }

}
