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

        [Newtonsoft.Json.JsonConstructor]
        public MoveBoots(Player curPlayer, List<Player> playersPassed){
            currentPlayer = curPlayer;
            this.playersPassed = playersPassed;
        }
    }
}