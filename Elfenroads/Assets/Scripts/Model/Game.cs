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
        public CounterPile counterPile { protected set; get; }

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
        protected Game(Board board, List<Player> players, Player startingPlayer,
                        GamePhase currentPhase, Variant variant, CardPile cards,
                        CardPile discardPile, CounterPile counterPile) {
            this.board = board;
            this.players = players;
            this.startingPlayer = startingPlayer;
            this.currentPhase = currentPhase;
            this.variant = variant;
            this.cards = cards;
            this.discardPile = discardPile;
            this.counterPile = counterPile;
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

            if ( !startingPlayer.Equals(update.startingPlayer) ) {
                startingPlayer = (Player) ModelStore.Get(update.startingPlayer.id);
                modified = true;
            }

            // might fuck up ***
            if (currentPhase.isCompatible(update.currentPhase)) {
                if (currentPhase.Update(update.currentPhase)) {
                    modified = true;
                }
            }
            else {
                // needs more thought
                currentPhase.End();
                currentPhase = update.currentPhase;
                currentPhase.Update(update.currentPhase);
                modified = true;
            }

            if (variant != update.variant) {
                variant = update.variant;
                modified = true;
            }

            if (cards.Update(update.cards)) {
                modified = true;
            }

            if (discardPile.Update(update.discardPile)) {
                modified = true;
            }

            if (counterPile.Update(update.counterPile)) {
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
            Elfengold = 1,
            LongerGame = 2, //Elfenland or Elfengold.
            HomeTown = 4, //Elfenland or Elfengold
            RandomGoldTokens = 8, //Elfengold only.
            ElfenWitch = 16 //Elfengold only.
        }
    }
}