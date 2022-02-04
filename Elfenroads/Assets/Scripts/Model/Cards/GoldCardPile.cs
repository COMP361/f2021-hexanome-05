using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class GoldCardPile
    {
        public List<GoldCard> cards;

        public GoldCardPile(){
            cards = new List<GoldCard>();
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.
    }
}