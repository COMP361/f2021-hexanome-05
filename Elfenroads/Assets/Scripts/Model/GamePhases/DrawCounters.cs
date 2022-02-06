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
    }
}