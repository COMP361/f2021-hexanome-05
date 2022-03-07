using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class InfoButton : MonoBehaviour
{
    public GameObject targetWindow;

    void Start() {
        targetWindow.SetActive(false);
        }       
   
     public void ShowCanvas(){
        if(!targetWindow.activeSelf){
                targetWindow.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
            }
     }
}
