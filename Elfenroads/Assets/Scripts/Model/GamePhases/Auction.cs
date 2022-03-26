using Models.Helpers;
using System.Collections.Generic;
using System;

namespace Models
{
    public class Auction : GamePhase
    {
        override public event EventHandler Updated;
        public List<Counter> countersForAuction { protected set; get; }
        public int highestBid { protected set; get; }
        public Player highestBidder { protected set; get; }
        public Player currentPlayer { protected set; get; }

        public Auction(List<Counter> countersForAuction) {
            this.countersForAuction = new List<Counter>(countersForAuction);
            this.highestBid = 0;
            this.highestBidder = null;
        }

        public Counter getCurrentAuctioningCounter() {
            return countersForAuction[0];
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Auction(List<Counter> countersForAuction, int highestBid, Player highestBidder, Player currentPlayer) {
            this.countersForAuction = countersForAuction;
            this.highestBid = highestBid;
            this.highestBidder = highestBidder;
            this.currentPlayer = currentPlayer;
        }

        override public bool isCompatible(GamePhase update) {
            return update as Auction != null;
        }

        override public bool Update(GamePhase update) {
            Auction updateTypecast = update as Auction;
            bool modified = false;

            if (countersForAuction.Update(updateTypecast.countersForAuction)) {
                modified = true;
            }

            if ( !highestBid.Equals(updateTypecast.highestBid) ) {
                highestBid = updateTypecast.highestBid;
                modified = true;
            }

            if ( !highestBidder.Equals(updateTypecast.highestBidder) ) {
                highestBidder = (Player) ModelStore.Get(updateTypecast.highestBidder.id);
                modified = true;
            }

            if ( !currentPlayer.Equals(updateTypecast.currentPlayer) ) {
                currentPlayer = (Player) ModelStore.Get(updateTypecast.currentPlayer.id);
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}
