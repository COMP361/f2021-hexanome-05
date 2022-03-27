using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class DrawCounters : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer { protected set; get; }

        
        // public DrawCounters(Player currentPlayer){
        //     this.currentPlayer = currentPlayer;
        //     playersPassed = new List<Player>();
        // }

        [Newtonsoft.Json.JsonConstructor]
        protected DrawCounters(Player currentPlayer){
            this.currentPlayer = currentPlayer;
        }

        override public bool isCompatible(GamePhase update) {
            return update as DrawCounters != null;
        }

        override public bool Update(GamePhase update) {
            DrawCounters updateTypecast = update as DrawCounters;
            bool modified = false;


            if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
                currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
                modified = true;
            }

            // if (playersPassed.Update(updateTypecast.playersPassed)) {
            //     modified = true;
            // }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}