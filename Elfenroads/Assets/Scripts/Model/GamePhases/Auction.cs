using Models.Helpers;
using System.Collections.Generic;
using System;

namespace Models
{
    public class Auction : GamePhase, IUpdatable<Auction>
    {
        public event EventHandler Updated;
        public List<Counter> countersForAuction { protected set; get; }
        public int highestBid { protected set; get; }
        public Player highestBidder { protected set; get; }

        public Auction(List<Counter> countersForAuction) {
            this.countersForAuction = new List<Counter>(countersForAuction);
            this.highestBid = 0;
            this.highestBidder = null;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Auction(List<Counter> countersForAuction, int highestBid, Player highestBidder) {
            this.countersForAuction = countersForAuction;
            this.highestBid = highestBid;
            this.highestBidder = highestBidder;
        }

        public bool Update(Auction update) {
            bool modified = false;

            if (countersForAuction.Update(update.countersForAuction)) {
                modified = true;
            }

            if ( !highestBid.Equals(update.highestBid) ) {
                highestBid = update.highestBid;
                modified = true;
            }

            if ( !highestBidder.Equals(update.highestBidder) ) {
                highestBidder = (Player) ModelStore.Get(update.highestBidder.id);
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}
