using System.Collections.Generic;
using UnityEngine;


namespace Models {
    public class Game {
        private Board board;
        private List<Player> players;
        private Player currentPlayer;
        //Later: Add currentPhase, currentRound, isElfenGold, phaseLoops, and variants. (and startingPlayer)

        public Game(List<GameObject> roadObjects, List<GameObject> townObjects) {
            this.board = new Board(roadObjects, townObjects);
        }

        public void createPlayers(List<string> playerNames){
            foreach(string name in playerNames){
                Player newPlayer = new Player(name, Color.NONE, board.getTownByName("Elfenhold"));
                players.Add(newPlayer);
            }
        }

    }
}