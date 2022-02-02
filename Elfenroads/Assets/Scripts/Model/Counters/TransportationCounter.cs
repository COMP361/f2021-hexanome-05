using System.Collections;
using System.Collections.Generic;
using Models;
using System;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public CardType cardType { set; get; }

        public TransportationCounter(Guid id, CardType cardType){
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