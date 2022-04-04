using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class SessionInfo 
{

    private Client thisClient;
    public string savegame_id {set; get;}
    public bool isElfenGold {set; get;}
    public bool isLongerGame {set; get;}
    public bool isHomeTown {set; get;}
    public bool isElvenWitch {set; get;}
    public bool isRandomDistribution {set; get;}

    private static SessionInfo instance; // Using the singleton pattern

    // returns singleton instance.
    public static SessionInfo Instance() {
        if (instance == null) {
            instance = new SessionInfo();
        }

        return instance;
    }

    public SessionInfo(){
        isElfenGold = false;
        isLongerGame = false;
        isHomeTown = false;
        isElvenWitch = false;
        isRandomDistribution = false;
        savegame_id = "";
    }

    public void resetSingleton(){
        instance = null;
    }

    public void setClient(){
        thisClient = Client.Instance();
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
            if (isElvenWitch) {
                variant |= Game.Variant.ElfenWitch;
            }
            if (isRandomDistribution) {
                variant |= Game.Variant.RandomGoldTokens;
            }
        }
        else {
            if (isLongerGame) {
                variant |= Game.Variant.LongerGame;
            }
        }

        if (isHomeTown) {
            variant |= Game.Variant.HomeTown;
        }

        return variant;
    }
}
