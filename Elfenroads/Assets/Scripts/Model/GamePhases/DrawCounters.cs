using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class DrawCounters : GamePhase 
    {
        override public event EventHandler Updated;
        public List<Counter> faceUpCounters { protected set; get; }
        public Player currentPlayer { protected set; get; }
        public List<Player> playersPassed { protected set; get; }

        
        public DrawCounters(Player currentPlayer){
            this.currentPlayer = currentPlayer;
            faceUpCounters = new List<Counter>();
            playersPassed = new List<Player>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected DrawCounters(Player currentPlayer, List<Player> playersPassed, List<Counter> faceUpCounters){
            this.currentPlayer = currentPlayer;
            this.playersPassed = playersPassed;
            this.faceUpCounters = faceUpCounters;
        }

        override public bool isCompatible(GamePhase update) {
            return update as DrawCounters != null;
        }

        override public bool Update(GamePhase update) {
            DrawCounters updateTypecast = update as DrawCounters;
            bool modified = false;

            if (faceUpCounters.Update(updateTypecast.faceUpCounters)) {
                modified = true;
            }

            if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
                currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
                modified = true;
            }

            if (playersPassed.Update(updateTypecast.playersPassed)) {
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}