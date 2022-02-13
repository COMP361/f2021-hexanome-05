using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class MoveBoots : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer;
        public List<Player> playersPassed;

        public MoveBoots(Player currentPlayer){
            this.currentPlayer = currentPlayer;
            playersPassed = new List<Player>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected MoveBoots(Player currentPlayer, List<Player> playersPassed){
            this.currentPlayer = currentPlayer;
            this.playersPassed = playersPassed;
        }

        override public bool isCompatible(GamePhase update) {
            return update as MoveBoots != null;
        }

        override public bool Update(GamePhase update) {
            MoveBoots updateTypecast = update as MoveBoots;
            bool modified = false;

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