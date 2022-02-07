using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class CounterPile : IUpdatable<CounterPile>
    {
        public event EventHandler Updated;
        public List<Counter> counters { protected set; get; }

        public CounterPile(){
            this.counters = new List<Counter>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected CounterPile(List<Counter> counters) {
            this.counters = counters;
        }

        public bool Update(CounterPile update) {
            if (counters.Update(update.counters)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}