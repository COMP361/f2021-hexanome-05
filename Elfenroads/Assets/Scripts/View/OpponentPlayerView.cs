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
        updateTexts();
    }

    public void updateTexts(){
        Player curPlayer = Elfenroads.Model.getCurrentPlayer();
        if(curPlayer == null){
            stats.text = "P: " + playerModel.inventory.townPieces.Count;
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                stats.text += " G: " + playerModel.inventory.gold;
            }
        }else if(curPlayer.id == playerModel.id){
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                stats.text = "P: " + playerModel.inventory.townPieces.Count + " G: " + playerModel.inventory.gold  + "\nCurrentPlayer: Y";
            }else{
                stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: Y";
            }
        }else{
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                stats.text = "P: " + playerModel.inventory.townPieces.Count + " G: " + playerModel.inventory.gold  + "\nCurrentPlayer: N";
            }else{
                stats.text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: N";
            }
        } 
    }

    public void expandThisInventory(){
        playerInfoController.openAndSetupWindow(playerModel);
    }
}
