using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantSelectorScript : MonoBehaviour
{
    public bool isElfenGold {set; get;}
    public bool isLongerGame {set; get;}
    public bool isHomeTown {set; get;}
    public bool isElvenWitch {set; get;}
    public bool isRandomDistribution {set; get;}

    void Update(){
        if(isElfenGold){
            SessionInfo.Instance().isElfenGold = isElfenGold;
        }

        if(isLongerGame){
            SessionInfo.Instance().isLongerGame = isLongerGame;
        }

        if(isHomeTown){
            SessionInfo.Instance().isHomeTown = isHomeTown;
        }

        if(isElvenWitch){
            SessionInfo.Instance().isElvenWitch = isElvenWitch;
        }

        if(isRandomDistribution){
            SessionInfo.Instance().isRandomDistribution = isElvenWitch;
        }
    }
}
