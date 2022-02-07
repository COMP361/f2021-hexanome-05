using System.Collections.Generic;
using System;
namespace Models
{
    public class MoveBoots : GamePhase 
    {
        public Player currentPlayer;
        public List<Player> playersPassed;

        public MoveBoots(Player curPlayer){
            currentPlayer = curPlayer;
            playersPassed = new List<Player>();
        }
    }
}