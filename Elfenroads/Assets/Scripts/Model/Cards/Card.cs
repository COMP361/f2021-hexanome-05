using System;
using Models;

namespace Models
{
    public abstract class Card
    {
        public Guid id { protected set; get; }

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