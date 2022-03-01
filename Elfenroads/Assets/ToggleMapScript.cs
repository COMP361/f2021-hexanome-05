using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class ToggleMapScript : MonoBehaviour
{
    public GameObject DrawCountersCanvas;
    

    public void toggleCanvas(){
        if(Elfenroads.Model.game.currentPhase is DrawCounters){
            if(DrawCountersCanvas.activeSelf){
                DrawCountersCanvas.SetActive(false);
                Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            }else{
                DrawCountersCanvas.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
