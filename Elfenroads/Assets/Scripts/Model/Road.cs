using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Helpers;
using System;


namespace Models {
    public class Road : GuidModel, IUpdatable<Road> {
        public event EventHandler Updated;
        public Town start { protected set; get; }
        public Town end { protected set; get; }
        public TerrainType roadType { protected set; get; }
        public List<Counter> counters { protected set; get; }

        public Road(Town start, Town end, TerrainType roadType) : base() {
            this.start = start;
            this.end = end;
            this.roadType = roadType;
            this.counters = new List<Counter>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Road(Town start, Town end, TerrainType roadType, List<Counter> counters, Guid id) : base(id) {
            this.start = start;
            this.end = end;
            this.roadType = roadType;
            this.counters = new List<Counter>(counters);
        }

        public bool Update(Road update) {
            if (counters.Update(update.counters)) {
                Updated?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }
    }
}