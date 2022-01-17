using System.Collections.Generic;


namespace Models {
    public class Game {
        private Board board;
        private List<Player> players;
        private Player currentPlayer;
        //Later: Add currentPhase, currentRound, isElfenGold, phaseLoops, and variants. (and startingPlayer)

        public Game(Board board) {
            this.board = board;
        }

        public void createPlayers(List<string> playerNames){
            int bootId = 0;
            foreach(string name in playerNames){
                Player newPlayer = new Player(name, Color.RED, bootId);
                players.Add(newPlayer);
                bootId++;
            }
        }

        public void SetBoard(Board board) {
            this.board = board;
        }

    }
}