using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public class Session 
{
    public string sessionID;
    public string hostPlayerName;
    public List<string> players;
    public bool launched; 
    public string saveID;
    public int maxPlayers;

    public Session(string aSessionID, string aHostName, string[] aPlayers, bool aLaunchState, string saveId, int maxPlayers){
        sessionID = aSessionID;
        hostPlayerName = aHostName;
        //Now, we need to parse the latter two stings into an array and a bool.
        //Need helper which will get a list of all strings within a string surrounded by " " 
        players = new List<string>();
        foreach(string s in aPlayers){
            players.Add(s);
        }
        // MatchCollection mc = Regex.Matches(aPlayers, "(?:\").*?(?:\")");
        // foreach(Match m in mc){
        //     players.Add(m.ToString().Trim('"'));
        // }
        saveID = saveId;
        launched = aLaunchState;
        this.maxPlayers = maxPlayers;
    }
}
