using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class Auction
    {
        public List<Counter> countersForAuction;
        //public Counter currentItem; //I guess we could just have the item at index 0 of "countersForAuction" be the currentItem.
        public int highestBid;
        public Player highestBidder;

        public Auction(){
            countersForAuction = new List<Counter>();
            highestBid = 0;
            highestBidder = null;
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! (we could just show the static image of the back of the deck instead) ***
        //Needs an "Update" function. Elfengold, so low priority.
    }
}
