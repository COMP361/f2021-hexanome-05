using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class GameOver : GamePhase 
    {
        override public event EventHandler Updated;
        public List<Player> playAgain { protected set; get; }
        public List<int> scores { protected set; get; }

        
        public GameOver(){
            playAgain = new List<Player>();
            scores = new List<int>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected GameOver(List<Player> playAgain, List<int> scores){
            this.playAgain = playAgain;
            this.scores = scores;
        }

        override public bool isCompatible(GamePhase update) {
            return update as DrawCounters != null;
        }

        override public bool Update(GamePhase update) {
            GameOver updateTypecast = update as GameOver;
            bool modified = false;

            // if (faceUpCounters.Update(updateTypecast.faceUpCounters)) {
            //     modified = true;
            // }

            // if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
            //     currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
            //     modified = true;
            // }

            // if (playAgain.Update(updateTypecast.playersPassed)) {
            //     modified = true;
            // }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}