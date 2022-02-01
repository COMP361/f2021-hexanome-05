using System.Collections.Generic;
using UnityEngine; //Only here for Debug.Logs


namespace Models {
    public class Game {
        private Board board;
        private List<Player> players;
        private Player currentPlayer;
        //Later: Add currentPhase, currentRound, isElfenGold, phaseLoops, and variants. (and startingPlayer)

        public Game(Board board) {
            this.board = board;
            this.players = new List<Player>();
        }

        public void createPlayerTest(){
            int bootId = 0;
            Player newPlayer = new Player("test", Color.RED, System.Guid.Empty); //Player is set to red here, should be changed later.
            players.Add(newPlayer);
            Elfenroads.Model.curPlayer = newPlayer;
        
        }

        public void SetBoard(Board board) {
            this.board = board;
        }

    }
}