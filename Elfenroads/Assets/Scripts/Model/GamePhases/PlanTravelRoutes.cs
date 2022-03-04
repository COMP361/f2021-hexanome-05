using System.Collections.Generic;
using System;
using Models.Helpers;

namespace Models
{
    public class PlanTravelRoutes : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer;
        public List<Player> playersPassed;

        public PlanTravelRoutes(Player currentPlayer){
            this.currentPlayer = currentPlayer;
            playersPassed = new List<Player>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected PlanTravelRoutes(Player currentPlayer, List<Player> playersPassed){
            this.currentPlayer = currentPlayer;
            this.playersPassed = playersPassed;
        }

        override public bool isCompatible(GamePhase update) {
            return update as PlanTravelRoutes != null;
        }

        override public bool Update(GamePhase update) {
            PlanTravelRoutes updateTypecast = update as PlanTravelRoutes;
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