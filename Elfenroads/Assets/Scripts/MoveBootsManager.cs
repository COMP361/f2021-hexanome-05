using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBootsManager : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        //This line adds this script as a kind of "observer" to any changes in game state in the GameManager.
        GameManager.onBroadStateChange += GameManagerOnGameStateChanged;
    }

    //This function does something depending on the aforementioned game state change.
    void GameManagerOnGameStateChanged(BroadGameState state){
        if(state == BroadGameState.MoveBoot){
            Debug.Log("Time for MoveBoots!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
