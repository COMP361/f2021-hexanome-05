using System.Collections;
using System.Collections.Generic;
using Models;
using System;


namespace Models {
    public class Road : GuidModel, IUpdatable<Road> {
        public event EventHandler Updated;
        public Town start { private set; get; }
        public Town end { private set; get; }
        public TerrainType roadType { private set; get; }
        public List<Counter> counters { private set; get; }

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

        public void Update(Road update) {
            if ()
        }
    }
}