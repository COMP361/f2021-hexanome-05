using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class DrawCards : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer { protected set; get; }

        
        // public DrawCards(Player currentPlayer){
        //     this.currentPlayer = currentPlayer;
        // }

        [Newtonsoft.Json.JsonConstructor]
        protected DrawCards(Player currentPlayer){
            this.currentPlayer = currentPlayer;
        }

        override public bool isCompatible(GamePhase update) {
            return update as DrawCards != null;
        }

        override public bool Update(GamePhase update) {
            DrawCards updateTypecast = update as DrawCards;
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