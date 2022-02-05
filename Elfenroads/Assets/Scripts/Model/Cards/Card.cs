using System;
using Models;

namespace Models
{
    public abstract class Card : GuidModel
    {
        protected Card() : base() {}
        protected Card(Guid id) : base(id) {}
    }
}