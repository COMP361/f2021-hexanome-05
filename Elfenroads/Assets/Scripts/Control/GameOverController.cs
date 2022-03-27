using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class GameOverController : MonoBehaviour
{

    public TMPro.TMP_Text winnerText;
    public TMPro.TMP_Text leaderBoardText;

    public void updateTexts(){
        List<KeyValuePair<int, string>> scores = new List<KeyValuePair<int, string>>();
        
        foreach(Player p in Elfenroads.Model.game.players){
            scores.Add(new KeyValuePair<int, string>(p.inventory.townPieces.Count, p.name));
        }
        scores.Sort(new ScoreComparer());
        scores.Reverse();

        // int highestScore = -1;

        // foreach(KeyValuePair<int, string> kvp in scores){
        //     if(kvp.Key > highestScore){
        //         highestScore = kvp.Key;
        //     }
        // }
        // List<string> winners = new List<string>();
        // foreach(KeyValuePair<int, string> kvp in scores){
        //     if(kvp.Key == highestScore){
        //         winners.Add(kvp.Value);
        //     }
        // }

        int highestScore = -1;

        foreach(int score in ((GameOver) Elfenroads.Model.game.currentPhase).scores){
            if(score > highestScore){
                highestScore = score;
            }
        }
        List<string> winners = new List<string>();
        for(int j = 0 ; j < ((GameOver) Elfenroads.Model.game.currentPhase).scores.Count ; j++){
            if(((GameOver) Elfenroads.Model.game.currentPhase).scores[j] == highestScore){
                winners.Add(Elfenroads.Model.game.players[j].name);
            }
        }

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
        //leaderBoardText.text = getScoresString(scores);
        leaderBoardText.text = getScoresVariant();

        for(int i = 0 ; i < ((GameOver) Elfenroads.Model.game.currentPhase).scores.Count ; i++ ){
            int curScore = ((GameOver) Elfenroads.Model.game.currentPhase).scores[i];
            string playerName = Elfenroads.Model.game.players[i].name;
            Debug.Log(playerName + " has score " + curScore);
        }
    }


    public string getScoresString(List<KeyValuePair<int,string>> scores){
		string results = "Leaderboard: ";
		int curScore = -1;
		int position = 0;
		for(int i=0; i<scores.Count; i++){
			KeyValuePair<int,string> curKVP = scores[i];

			if((curScore != curKVP.Key)){
				curScore = curKVP.Key;
				position++;
				results += "\n" + position + ": " + curKVP.Value + " with " + curKVP.Key + " points!";
			}else{
				results += "\n" + position + ": " + curKVP.Value + " with " + curKVP.Key + " points!";
			}
		}

		return results;
	}

    public string getScoresVariant(){
        string results = "Leaderboard: ";
		int curScore = -1;
		int position = 0;
		for(int i=0; i<((GameOver) Elfenroads.Model.game.currentPhase).scores.Count; i++){
			int nowScore = ((GameOver) Elfenroads.Model.game.currentPhase).scores[i];
            Player curPlayer = Elfenroads.Model.game.players[i];

			if((curScore != nowScore)){
				curScore = nowScore;
				position++;
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points!";
			}else{
				results += "\n" + position + ": " + curPlayer.name + " with " + nowScore + " points!";
			}
		}
		return results;
    }


    private class ScoreComparer : IComparer<KeyValuePair<int, string>>{
        public int Compare(KeyValuePair<int, string> kv1, KeyValuePair<int, string> kv2){
            return kv1.Key.CompareTo(kv2.Key);
        }
    }
}
