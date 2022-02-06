using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class GoldCardPile : IUpdatable<FaceUpCards>
    {
        public event EventHandler Updated;
        public List<Card> cards { protected set; get; }

        public GoldCardPile() {
            this.cards = new List<Card>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected GoldCardPile(List<Card> cards) {
            this.cards = cards;
        }

        public bool Update(GoldCardPile update) {
            if (cards.Update(update.cards)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}