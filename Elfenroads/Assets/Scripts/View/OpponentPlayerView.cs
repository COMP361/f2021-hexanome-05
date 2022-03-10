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
        Player curPlayer = Elfenroads.Model.getCurrentPlayer();
        if(curPlayer == null){
            stats.text = "P: " + playerModel.inventory.townPieces.Count;
        }else if(curPlayer.id == playerModel.id){
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: Y";
        }else{
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: N";
        } 
    }

    public void updateTexts(){
        Player curPlayer = Elfenroads.Model.getCurrentPlayer();
        if(curPlayer == null){
            stats.text = "P: " + playerModel.inventory.townPieces.Count;
        }else if(curPlayer.id == playerModel.id){
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: Y";
        }else{
            stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: N";
        }
    }

    public void expandThisInventory(){
        playerInfoController.openAndSetupWindow(playerModel);
    }
}
