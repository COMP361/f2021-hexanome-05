using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class GoldCardPile : IUpdatable<GoldCardPile>
    {
        public event EventHandler Updated;
        public List<GoldCard> cards { protected set; get; }

        public GoldCardPile() {
            this.cards = new List<GoldCard>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected GoldCardPile(List<GoldCard> cards) {
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