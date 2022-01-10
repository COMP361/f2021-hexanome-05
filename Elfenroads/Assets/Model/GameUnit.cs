
// Model/Class for games
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Models

{
    public class GameUnit
    {
        private GameObject GameObject { get; set; }

        protected GameUnit(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }
    }
}