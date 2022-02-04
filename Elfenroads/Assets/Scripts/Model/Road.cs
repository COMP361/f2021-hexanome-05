using System.Collections;
using System.Collections.Generic;
using Models;
using System;


namespace Models {
    public class Road : INotifyModelUpdated {
        public readonly int id;
        public event EventHandler ModelUpdated;
        public Town start { private set; get; }
        public Town end { private set; get; }
        public TerrainType roadType { private set; get; }
        public List<Counter> counters { get; set; }

        public Road(Town start, Town end, TerrainType roadtype, int id) {
            this.start = start;
            this.end = end;
            this.roadType = roadType;
            this.id = id;

            //start.connectRoad(this); //See Town.cs
            //end.connectRoad(this);
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.

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