using System;
using System.Collections;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class CardPile : IUpdatable<CardPile>
    {
        public event EventHandler Updated;
        public List<Card> cards { protected set; get; }

        public CardPile() {
            cards = new List<Card>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected CardPile(List<Card> cards) : this() {
            this.cards = cards;
        }

        public bool Update(CardPile update) {
            if (cards.Update(update.cards)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}