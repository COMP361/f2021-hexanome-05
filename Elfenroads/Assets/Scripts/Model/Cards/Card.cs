using System;
using Models;

namespace Models
{
    public abstract class Card
    {
        public Guid id { set; get; }
        public int round; //Cards have rounds. It may be worth setting some kind of sorting order (items with lower rounds show up first in the CardPile list).

        public bool Equals(Counter other) {
            return other != null &&
                    other.id == this.id;
        }

        public override bool Equals(object obj)
        {   
            return Equals(obj as Counter);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}