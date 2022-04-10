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
        SessionInfo.Instance().isElfenGold = isElfenGold;
        SessionInfo.Instance().isLongerGame = isLongerGame;
        SessionInfo.Instance().isHomeTown = isHomeTown;
        SessionInfo.Instance().isElvenWitch = isElvenWitch;
        SessionInfo.Instance().isRandomDistribution = isRandomDistribution;
    }
}
