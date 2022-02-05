using System;

namespace Models
{
    public abstract class Counter : GuidModel
    {
        public bool isFaceUp = true;

        public Counter() : base() {}

        protected Counter(Guid id) : base(id) {}
    }
} 