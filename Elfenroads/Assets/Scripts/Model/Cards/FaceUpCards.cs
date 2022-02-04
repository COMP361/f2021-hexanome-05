using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class FaceUpCards
    {
        public List<Card> cards;

        public FaceUpCards(){
            cards = new List<Card>(3); //There can only be 3 cards at a time.
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.
    }
}