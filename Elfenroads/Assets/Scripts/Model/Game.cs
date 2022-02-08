using System.Collections.Generic;
using System;
using Models.Helpers;


namespace Models {
    public class Game : IUpdatable<Game> {
        public event EventHandler Updated;
        public Board board { protected set; get; }
        public List<Player> players { protected set; get; }

        // This signifies the player holding the "starting player figurine"
        // (see game rules) and decides who goes first on each phase of a round.
        public Player startingPlayer { protected set; get; }
        public GamePhase currentPhase {  set; get; }
        public Variant variant { protected set; get; }
        public CardPile cards { protected set; get; }
        public CardPile discardPile { protected set; get;}
        public CounterPile counters { protected set; get; }

        public Game(Board board) {
            this.board = board;
            this.players = new List<Player>();
        }

        // we should be using this constructor
        public Game(Board board, List<Player> players, Player startingPlayer, Variant variant) {
            this.board = board;
            this.players = new List<Player>(players);
            this.startingPlayer = startingPlayer;
            // this.currentPhase = new GamePhase(...)
            this.variant = variant;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Game(Board board, List<Player> players, Player startingPlayer, GamePhase currentPhase, Variant variant) {
            this.board = board;
            this.players = players;
            this.startingPlayer = startingPlayer;
            this.currentPhase = currentPhase;
            this.variant = variant;
        }

        public Player GetPlayer(string name) {
            foreach (Player player in players) {
                if (player.name == name) {
                    return player;
                }
            }

            return null;
        }

        public bool Update(Game update) {
            bool modified = false;

            if (board.Update(update.board)) {
                modified = true;
            }

            if (players.DeepUpdate(update.players)) {
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
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