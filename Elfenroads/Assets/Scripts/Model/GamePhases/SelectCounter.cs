using System.Collections.Generic;
using System;
using Models.Helpers;
using Models;

namespace Models
{
    public class SelectCounter : GamePhase 
    {
        override public event EventHandler Updated;
        public Player currentPlayer { protected set; get; }
        public List<Counter> counters { protected set; get; }

        [Newtonsoft.Json.JsonConstructor]
        protected SelectCounter(Player currentPlayer, List<Counter> counters){
            this.currentPlayer = currentPlayer;
            this.counters = counters;
        }

        override public bool isCompatible(GamePhase update) {
            return update as SelectCounter != null;
        }

        override public bool Update(GamePhase update) {
            SelectCounter updateTypecast = update as SelectCounter;
            bool modified = false;


            if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
                currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
                modified = true;
            }

            if (counters.Update(updateTypecast.counters)) {
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}