using System;
using System.Collections.Generic;
using Models.Helpers;

namespace Models
{
    public class FaceUpCounters : IUpdatable<FaceUpCounters>
    {
        public event EventHandler Updated;
        public List<Counter> counters { protected set; get; }

        public FaceUpCounters() {
            this.counters = new List<Counter>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected FaceUpCounters(List<Counter> counters) {
            this.counters = counters;
        }

        public bool Update(FaceUpCounters update) {
            if (counters.Update(update.counters)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}