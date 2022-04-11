using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class GameOverController : MonoBehaviour
{

    public TMPro.TMP_Text winnerText;
    public TMPro.TMP_Text leaderBoardText;

    public void updateTexts(){

        string playerName = SessionInfo.Instance().getClient().clientCredentials.username;
        if(playerName == SessionInfo.Instance().getClient().mySession.hostPlayerName){
            //The host sends a "quit" request to the server, after a few seconds.
            Invoke("endOfGameQuit", 3f);
        }
        //Then, sockets are turned off.
        Elfenroads.Control.stopListening();

        //Represents player score and name.
        List<KeyValuePair<int, Player>> scores = new List<KeyValuePair<int, Player>>();
        
        for(int i = 0 ; i < Elfenroads.Model.game.players.Count; i++){
            scores.Add(new KeyValuePair<int, Player>(((GameOver) Elfenroads.Model.game.currentPhase).scores[i], Elfenroads.Model.game.players[i]));
        }
        scores.Sort(new ScoreComparer());
        scores.Reverse(); //Scores should now be in order.

        //Setup things for score calculation.
        List<string> winners = new List<string>();
        int highestScore = -1;
        int highestCards = -1;

        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            //If we're in elfengold, need to take gold into account as well.
            int highestGold = -1;
            foreach(KeyValuePair<int, Player> kvp in scores){
                //If the key is greater than the current highest score.
                if(kvp.Key > highestScore){
                    highestScore = kvp.Key; //Set the new highest score, gold and cards.
                    highestGold = kvp.Value.inventory.gold;
                    highestCards = kvp.Value.inventory.cards.Count;
                }
                if(kvp.Key == highestScore){ 
                    if(kvp.Value.inventory.gold > highestGold){
                        highestGold = kvp.Value.inventory.gold; //Set the new highest gold and cards.
                        highestCards = kvp.Value.inventory.cards.Count;
                    }
                    if(kvp.Value.inventory.gold == highestGold){
                        if(kvp.Value.inventory.cards.Count > highestCards){
                        highestCards = kvp.Value.inventory.cards.Count; //Set the new highest cards.
                        }
                    }
                }
            }

            for(int j = 0 ; j < scores.Count; j++){
                if((scores[j].Key == highestScore) && (scores[j].Value.inventory.gold == highestGold) && (scores[j].Value.inventory.cards.Count == highestCards) ){
                    winners.Add(scores[j].Value.name);
                }
            }
            leaderBoardText.text = fillLeaderboardElfengold(scores);

        }else{
            foreach(KeyValuePair<int, Player> kvp in scores){
                //If we've found a new high score, the highestCards (which represent the most cards among the winning players) are changed as well.
                if(kvp.Key > highestScore){
                    highestScore = kvp.Key;
                    highestCards = kvp.Value.inventory.cards.Count;
                }
                //If we're equal to the highest score, check for highestCards.
                if(kvp.Key == highestScore){ 
                    if(kvp.Value.inventory.cards.Count > highestCards){
                        highestCards = kvp.Value.inventory.cards.Count;
                    }
                }
            }
            //Now we know the highest score, plus the highest amount of cards from those who have the highest score. So display the winners now:
            for(int j = 0 ; j < scores.Count; j++){
                if((scores[j].Key == highestScore) && (scores[j].Value.inventory.cards.Count == highestCards)){
                    winners.Add(scores[j].Value.name);
                }
            }

            //Now we must fill the leaderboard.
            leaderBoardText.text = fillLeaderboardElfenland(scores);
        }

        // foreach(int score in ((GameOver) Elfenroads.Model.game.currentPhase).scores){
        //     if(score > highestScore){
        //         highestScore = score;
        //     }
        // }
        // List<string> winners = new List<string>();
        // for(int j = 0 ; j < ((GameOver) Elfenroads.Model.game.currentPhase).scores.Count ; j++){
        //     if(((GameOver) Elfenroads.Model.game.currentPhase).scores[j] == highestScore){
        //         winners.Add(Elfenroads.Model.game.players[j].name);
        //     }
        // }

        string winText = "Congratulations ";
        if(winners.Count == 1){
            winText += winners[0] + " for the victory!";
        }else{
            for(int i = 0; i < winners.Count ; i++){
                string curWinner = winners[i];
                if(i == winners.Count - 2){
                    winText += winners[i] + " and ";
                }else if(i == winners.Count - 1){
                    winText += winners[i];
                }else{
                    winText += winners[i] + ", ";
                }
            }
            winText += " for the victory!";
        }
        winnerText.text = winText;
        //leaderBoardText.text = getScoresVariant();
    }

    public string fillLeaderboardElfenland(List<KeyValuePair<int,Player>> sortedScores){
        string results = "Leaderboard: ";
		int curScore = -1;
        int curCards = -1;
		int position = 0;
		for(int i=0; i < sortedScores.Count; i++){
			int nowScore = sortedScores[i].Key;
            Player curPlayer = sortedScores[i].Value;

			if((curScore != nowScore || curCards !=  curPlayer.inventory.cards.Count)){
				curScore = nowScore;
                curCards = curPlayer.inventory.cards.Count;
				position++;
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points and " + curPlayer.inventory.cards.Count + " cards!";
			}else{
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points and " + curPlayer.inventory.cards.Count + " cards!";
			}
		}
		return results;
    }

    public string fillLeaderboardElfengold(List<KeyValuePair<int,Player>> sortedScores){
        string results = "Leaderboard: ";
		int curScore = -1;
        int curCards = -1;
        int curGold = -1;
		int position = 0;
		for(int i=0; i < sortedScores.Count; i++){
			int nowScore = sortedScores[i].Key;
            Player curPlayer = sortedScores[i].Value;

			if((curScore != nowScore|| curCards !=  curPlayer.inventory.cards.Count || curGold != curPlayer.inventory.gold)){
				curScore = nowScore;
                curGold = curPlayer.inventory.gold;
                curCards = curPlayer.inventory.cards.Count;
				position++;
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points, " + curPlayer.inventory.gold + " gold and " + curPlayer.inventory.cards.Count + " cards!";
			}else{
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points, " + curPlayer.inventory.gold + " gold and " + curPlayer.inventory.cards.Count + " cards!";
			}
		}
		return results;
    }

    private void endOfGameQuit(){
        Elfenroads.Control.requestQuit();
    }


    private class ScoreComparer : IComparer<KeyValuePair<int, Player>>{
        public int Compare(KeyValuePair<int, Player> kv1, KeyValuePair<int, Player> kv2){

            //If two players have a tie, check other stuff.
            if(kv1.Key.CompareTo(kv2.Key) == 0){
                //In Elfengold, we check for amount of gold and then amount of travelCards to break ties.
                if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                    if(kv1.Value.inventory.gold.CompareTo(kv2.Value.inventory.gold) == 0){ 
                        return kv1.Value.inventory.cards.Count.CompareTo(kv2.Value.inventory.cards.Count);
                    }else{
                        return kv1.Value.inventory.gold.CompareTo(kv2.Value.inventory.gold);
                    }
                }else{ //In Elfenlands, we check for amount of travel cards to break ties.
                    return kv1.Value.inventory.cards.Count.CompareTo(kv2.Value.inventory.cards.Count);
                }
            }else{
                return kv1.Key.CompareTo(kv2.Key);
            }
        }
    }
}
