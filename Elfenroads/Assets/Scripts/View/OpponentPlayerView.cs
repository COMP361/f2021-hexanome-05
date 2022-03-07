using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
using UnityEngine.UI;


public class OpponentPlayerView : MonoBehaviour
{
    
    public TMPro.TMP_Text playerName;
    public TMPro.TMP_Text stats;
    private Player playerModel;
    private PlayerInfoController playerInfoController;

    void Start(){
        playerInfoController = GameObject.Find("PlayerInfoController").GetComponent<PlayerInfoController>();
    }

    public void setAndSubscribeToModel(Player inputPlayer){
         playerModel = inputPlayer;
         playerModel.Updated += onModelUpdated;
         onModelUpdated(null, null);
    }

    void onModelUpdated(object sender, EventArgs e) {
        playerName.text = playerModel.name;
        if(playerModel.id == Elfenroads.Model.game.startingPlayer.id){
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nStartPlayer: Y";
        }else{
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nStartPlayer: N";
        } 
    }

    public void expandThisInventory(){
        playerInfoController.openAndSetupWindow(playerModel);
    }
}
