using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System;


namespace Models {
    public class Road : INotifyModelUpdated {
        public readonly int id;
        public event EventHandler ModelUpdated;
        public Town start { private set; get; }
        public Town end { private set; get; }
        public TerrainType roadType { private set; get; }
        // to be implemented
        // public List<string> Counters { get; set; }
        public List<Counter> counters { get; set; }

        public Road(Town start, Town end, TerrainType roadtype, int id) {
            this.start = start;
            this.end = end;
            this.roadType = roadType;
            this.id = id;

            //start.connectRoad(this); //See Town.cs
            //end.connectRoad(this);
        }
    }

    public enum TerrainType {
        Plain,
        Forest,
        Mountain,
        Desert,
        Stream, // The "rule" for streams will be to have it flow from city1 -> city2. Create roads according to this rule and there shouldn't be a problem.
        Lake
    }
}