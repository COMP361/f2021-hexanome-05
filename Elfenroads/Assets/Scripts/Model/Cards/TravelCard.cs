using System;

namespace Models
{
    public class TravelCard : Card
    {
        public TransportType type { protected set; get; }

        public TravelCard(TransportType type) : base() {
            this.type = type;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected TravelCard(TransportType type, Guid id) : base(id) {
            this.type = type;
        }
    }
}