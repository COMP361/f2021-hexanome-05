using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class GameOverController : MonoBehaviour
{

    public TMPro.TMP_Text winnerText;
    public TMPro.TMP_Text leaderBoardText;

    public void updateTexts(){
        Player winner = null;

        foreach(Player p in Elfenroads.Model.game.players){
            if(winner == null){
                winner = p;
            }else if(p.inventory.townPieces.Count > winner.inventory.townPieces.Count){
                winner = p;
            }
        }

        winnerText.text = "Congratulations " + winner.name + " for the victory!";
    }
}
