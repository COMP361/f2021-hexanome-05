using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class CounterPile
    {
        public List<Card> counters;

        public CounterPile(){
            counters = new List<Card>();
        }

        //*** This MAY need to be attached to a Unity GameObject with an appropriate ViewScript! (we may be able to just show the static image of the back of a counter as "randomDraw" instead) ***
        //Needs an "Update" function.
    }
}