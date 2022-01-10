using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


namespace Models {
    public abstract class Road {
        public Town start { private set; get; }
        public Town end { private set; get; }
        // to be implemented
        // public List<string> Counters { get; set; }

        public Road(Town start, Town end) {
            this.start = start;
            this.end = end;

            start.connectRoad(this);
            end.connectRoad(this);
        }
    }
}