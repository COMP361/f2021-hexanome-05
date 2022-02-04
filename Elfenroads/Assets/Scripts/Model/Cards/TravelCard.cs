using System;
using Models;

namespace Models
{
    public class TravelCard : Card
    {
        private TransportType type;

        public TravelCard(TransportType type) {
            this.id = new Guid();
        }
    }
}