using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class FaceUpCards : IUpdatable<FaceUpCards>
    {
        public event EventHandler Updated;
        public List<Card> cards { protected set; get; }

        public FaceUpCards() {
            this.cards = new List<Card>(3); //There can only be 3 cards at a time.
        }

        [Newtonsoft.Json.JsonConstructor]
        protected FaceUpCards(List<Card> cards) {
            this.cards = cards;
        }

        public bool Update(FaceUpCards update) {
            if (cards.Update(update.cards)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}