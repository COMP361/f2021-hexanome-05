using System;
using Models;

namespace Models
{
    public class TravelCard : Card
    {
        public TransportType type;

        public TravelCard(TransportType type, int forRound) {
            this.id = new Guid();
            this.type = type;
            this.round = forRound;
        }
    }
}