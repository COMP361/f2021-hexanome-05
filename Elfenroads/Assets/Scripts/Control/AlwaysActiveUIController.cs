using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class AlwaysActiveUIController : MonoBehaviour
{

    public GameObject helpWindow;

	void Start()
	{
        helpWindow.SetActive(false);
	}

    public void CloseHelpWindow(){
        if(helpWindow.activeSelf){
            helpWindow.SetActive(false);
            Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
        }
    }

    public void ShowHelpWindow(){
        if(!helpWindow.activeSelf){
                helpWindow.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
        }
    }
}
