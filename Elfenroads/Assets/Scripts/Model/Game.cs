using System.Collections.Generic;
using System;


namespace Models {
    public class Game : IUpdatable<Game> {
        public Board board { protected set; get; }
        public List<Player> players { protected set; get; }

        // This signifies the player holding the "starting player figurine"
        // (see game rules) and decides who goes first on each phase of a round.
        public Player startingPlayer { protected set; get; }
        public GamePhase currentPhase { protected set; get; }
        public Variant variant { protected set; get; }

        public Game(Board board) {
            this.board = board;
            this.players = new List<Player>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Game(Board board, List<Player> players, Player startingPlayer, GamePhase currentPhase, Variant variant) {
            this.board = board;
            this.players = players;
            this.startingPlayer = startingPlayer;
            this.currentPhase = currentPhase;
            this.variant = Variant;
        }

        public void createPlayerTest() {
            Player newPlayer = new Player("test", Color.RED, System.Guid.Empty); //Player is set to red here, should be changed later.
            players.Add(newPlayer);
            Elfenroads.Model.curPlayer = newPlayer;
        
        }

        public void SetBoard(Board board) {
            this.board = board;
        }

        [Flags]
        public enum Variant {
            Elfenland = 0,
            Elfengold = 1 << 0,
            LongerGame = 1 << 1, //Elfenland or Elfengold.
            DestinationTown = 1 << 2, //Elfenland or Elfengold
            RandomGoldTokens = 1 << 3, //Elfengold only.
            ElfenWitch = 1 << 4 //Elfengold only.
        }
    }
}