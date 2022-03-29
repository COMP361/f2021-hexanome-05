using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class SessionInfoObj 
{

    private Client thisClient;
    public bool isElfenGold {set; get;}
    public bool isLongerGame {set; get;}
    public bool isHomeTown {set; get;}
    public bool isElvenWitch {set; get;}
    public bool isRandomDistribution {set; get;}

    private static SessionInfoObj instance; // Using the singleton pattern

    // returns singleton instance.
    public static SessionInfoObj Instance() {
        if (instance == null) {
            instance = new SessionInfoObj();
        }

        return instance;
    }

    public SessionInfoObj(){
        isElfenGold = false;
        isLongerGame = false;
        isHomeTown = false;
        isElvenWitch = false;
        isRandomDistribution = false;
    }

    public void wipeInfo(){
        isElfenGold = false;
        isLongerGame = false;
        isHomeTown = false;
        isElvenWitch = false;
        isRandomDistribution = false;
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
