using System.Collections.Generic;
using System;

namespace Models
{
    public class DrawCounters : GamePhase 
    {
        public List<Counter> faceUpCounters;
        public Player currentPlayer;
        public List<Player> playersPassed;

        
        public DrawCounters(Player curPlayer){
            currentPlayer = curPlayer;
            faceUpCounters = new List<Counter>();
            playersPassed = new List<Player>();
        }

        [Newtonsoft.Json.JsonConstructor]
        public DrawCounters(Player curPlayer, List<Player> playersPassed, List<Counter> counters){
            currentPlayer = curPlayer;
            this.playersPassed = playersPassed;
            this.faceUpCounters = counters;
        }

        
    }
}