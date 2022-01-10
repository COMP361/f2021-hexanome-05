using UnityEngine;
using System.Collections.Generic;


namespace Models {
    // not sure if we need this class - Dan
    public class GameUnit
    {
        private GameObject GameObject { get; set; }

        protected GameUnit(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }
    }
}