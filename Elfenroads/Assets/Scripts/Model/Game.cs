using System.Collections.Generic;
using UnityEngine; //Only here for Debug.Logs


namespace Models {
    public class Game {
        public Board board;

        public List<Player> players;
        public List<Player> playersPassed;
        public Player currentPlayer;
        public Player startingPlayer; //This signifies the player holding the "starting player figurine" (see game rules) and decides who goes first on each round of a phase.
        public bool isElfenGold;
        public GamePhase currentPhase;
        public List<Variant> variants;

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

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.

    }



    public enum GamePhase{
        DrawAdditionalCounters, //Elfenland. At the start of this phase, incorporate "DealTravelCards" and "Draw a Transportation Counter from the face down stack" phases from the game rules, since they require no player input.
        DrawCards, //Elfengold.
        SelectFaceDownCounter, //Elfengold. Also incorporate "Distribute Gold Coins" here since it requires no player input.
        Auction, //Elfengold.
        PlanTravel, //Elfenland or Elfengold.
        MoveBoots, //Elfenland or Elfengold.
        FinishRound //Elfenland or Elfengold.
    }

    public enum Variant{
        LongerGame, //Elfenland or Elfengold.
        DestinationTown, //Elfenland or Elfengold
        RandomGoldTokens, //Elfengold only.
        ElfenWitch //Elfengold only.
    }

}