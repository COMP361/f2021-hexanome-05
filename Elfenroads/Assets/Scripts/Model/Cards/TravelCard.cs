using System;

namespace Models
{
    public class TravelCard : Card
    {
        public TransportType transportType { protected set; get; }

        public TravelCard(TransportType transportType) : base() {
            this.transportType = transportType;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected TravelCard(TransportType transportType, Guid id) : base(id) {
            this.transportType = transportType;
        }
    }
}