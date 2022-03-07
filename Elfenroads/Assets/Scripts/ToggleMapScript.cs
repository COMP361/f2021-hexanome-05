using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class ToggleMapScript : MonoBehaviour
{
    public GameObject targetWindow;
    public PlayerInfoController playerInfoController;
    

    public void toggleCanvas(){
        //Don't want to allow toggling the map if a player inventory is open.
        if(playerInfoController.windowOpen){
            return;
        }

        if(Elfenroads.Model.game.currentPhase is DrawCounters || Elfenroads.Model.game.currentPhase is FinishRound){
            if(targetWindow.activeSelf){
                targetWindow.SetActive(false);
                Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            }else{
                targetWindow.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
