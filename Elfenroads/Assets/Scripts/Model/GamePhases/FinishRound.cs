using System.Collections.Generic;
using System;

namespace Models
{
    public class FinishRound : GamePhase 
    {
        //This is a "parallel" round, like ChooseBoot. All players do it at on their own. We may want to store the players who have finished their selection here anyway, for the Server's benefit?
        public FinishRound(){
        }
    }
}