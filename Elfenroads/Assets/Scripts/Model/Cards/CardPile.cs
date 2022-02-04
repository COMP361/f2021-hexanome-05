using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class CardPile
    {
        public List<Card> cards;

        public CardPile(){
            cards = new List<Card>();
        }

        //*** This will likely need to be attached to a Unity GameObject with an appropriate ViewScript! (we could just show the static image of the back of the deck instead) ***
        //Needs an "Update" function.
    }
}