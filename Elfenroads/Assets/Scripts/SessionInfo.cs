using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class SessionInfo : MonoBehaviour
{
    private Client thisClient;
    public bool isElfenGold {set; get;}
    public bool isLongerGame {set; get;}
    public bool isHomeTown {set; get;}

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

    public void setIsElfenGold(bool input){
        isElfenGold = input;
        Debug.Log("isElfenGold set to: " + input);
    }

    public Game.Variant getVariant() {
        Game.Variant variant = 0;
        
        if (isElfenGold) {
            variant |= Game.Variant.Elfengold;
        }
        if (isLongerGame) {
            variant |= Game.Variant.LongerGame;
        }
        if (isHomeTown) {
            variant |= Game.Variant.HomeTown;
        }

        return variant;
    }
}
