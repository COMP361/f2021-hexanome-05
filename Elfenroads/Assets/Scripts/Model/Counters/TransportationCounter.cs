using System.Collections;
using System.Collections.Generic;
using Models;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public CardType cardType { private set; get; }

        public TransportationCounter(int id, CardType cardType){
            this.id = id;
            this.cardType = cardType;
        }
    }

    public enum CardType{
        Pig,
        Cycle,
        Cloud,
        Unicorn,
        Troll,
        Dragon
    }
}