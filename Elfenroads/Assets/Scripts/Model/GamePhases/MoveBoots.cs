using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class MoveBoot : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer;

        public MoveBoot(Player currentPlayer){
            this.currentPlayer = currentPlayer;
        }

        // [Newtonsoft.Json.JsonConstructor]
        // protected MoveBoot(Player currentPlayer){
        //     this.currentPlayer = currentPlayer;
        // }

        override public bool isCompatible(GamePhase update) {
            return update as MoveBoot != null;
        }

        override public bool Update(GamePhase update) {
            MoveBoot updateTypecast = update as MoveBoot;
            bool modified = false;

            if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
                currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}