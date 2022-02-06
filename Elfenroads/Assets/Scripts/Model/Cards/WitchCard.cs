using System;

namespace Models
{
    public class WitchCard : Card
    {
        public WitchCard() : base() {}

        [Newtonsoft.Json.JsonConstructor]
        protected WitchCard(Guid id) : base(id) {}
    }
}