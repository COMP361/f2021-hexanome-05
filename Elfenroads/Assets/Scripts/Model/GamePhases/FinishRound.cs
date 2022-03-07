using System.Collections.Generic;
using System;

namespace Models
{
    public class FinishRound : GamePhase
    {
        override public event EventHandler Updated;
        //This is a "parallel" round, like ChooseBoot. All players do it at on their own. We may want to store the players who have finished their selection here anyway, for the Server's benefit?
        public FinishRound() {}

        //[Newtonsoft.Json.JsonConstructor]
        //protected FinishRound() {}

        override public bool isCompatible(GamePhase update) {
            return update as FinishRound != null;
        }

        override public bool Update(GamePhase update) {
            FinishRound updateTypecast = update as FinishRound;
            bool modified = false;

            // stuff to be added in the future

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}