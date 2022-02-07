using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class DiscardPile : IUpdatable<DiscardPile>
    {
        public event EventHandler Updated;
        public List<Card> cards { protected set; get; }

        public DiscardPile() {
            this.cards = new List<Card>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected DiscardPile(List<Card> cards) {
            this.cards = cards;
        }

        public bool Update(DiscardPile update) {
            if (cards.Update(update.cards)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}