using System;

namespace Models
{
    public class GoldCard : Card
    {
        public GoldCard() : base() {}

        [Newtonsoft.Json.JsonConstructor]
        protected GoldCard(Guid id) : base(id) {}
    }
}