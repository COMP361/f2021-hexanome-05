using System.Collections.Generic;
using System;
using Models.Helpers;
using UnityEngine; //Don't kill me Dan


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
        public List<Counter> faceUpCounters { protected set; get; }
        public List<Card> faceUpCards {protected set; get; }
        public int roundNumber { protected set; get; }
        public List<GoldCard> goldCardDeck { protected set; get; }

        public Game(Board board) {
            this.board = board;
            this.players = new List<Player>();
        }

        public Game(Board board, List<Player> players, Player startingPlayer, Variant variant, int roundNumber) {
            this.board = board;
            this.players = new List<Player>(players);
            this.startingPlayer = startingPlayer;
            // this.currentPhase = new GamePhase(...)
            this.variant = variant;
            this.roundNumber = 1;
            this.roundNumber = roundNumber;
            
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Game(Board board, List<Player> players, Player startingPlayer,
                        GamePhase currentPhase, Variant variant, CardPile cards,
                        CardPile discardPile, CounterPile counterPile, int roundNumber,
                        List<Counter> faceUpCounters, List<Card> faceUpCards, List<GoldCard> goldCardDeck ) {
            this.board = board;
            this.players = players;
            this.startingPlayer = startingPlayer;
            this.currentPhase = currentPhase;
            this.variant = variant;
            this.cards = cards;
            this.discardPile = discardPile;
            this.counterPile = counterPile;
            this.roundNumber = roundNumber;
            this.faceUpCounters = faceUpCounters;
            this.faceUpCards = faceUpCards;
            this.goldCardDeck = goldCardDeck;
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

            if (faceUpCounters.Update(update.faceUpCounters)) {
                modified = true;
            }

             if(faceUpCards.Update(update.faceUpCards)){ 
                 modified = true;
             }

             if(goldCardDeck.Update(update.goldCardDeck)){
                 Debug.Log("GoldCard deck modified");
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

            if(roundNumber != update.roundNumber){
                roundNumber = update.roundNumber;
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
            LongerGame = 2, //Elfenland only
            HomeTown = 4, //Elfenland or Elfengold
            RandomGoldTokens = 8, //Elfengold only.
            ElfenWitch = 16 //Elfengold only.
        }
    }
}