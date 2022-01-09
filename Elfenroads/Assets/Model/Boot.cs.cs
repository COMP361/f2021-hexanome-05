
// Model/Class for games


using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Boot : GameUnit
    {
        private Town CurrentTown;
        public Vector3 Offset;
        public Sprite BlueSprite;
        private BootColor BootColor;


        Boot(Town startingTown, BootColor bootColor, GameObject boot): base(boot)
        {
            this.CurrentTown = startingTown;
            this.BootColor = bootColor;
        }


        public void setCurrentCity(Town town)
        {
            this.CurrentTown = town;
        }

        public Town getCurrentTown()
        {
            return this.CurrentTown;
        }

        public void setColor(BootColor pColor)
        {
            this.BootColor = pColor;
        }

        public BootColor getColor()
        {
            return this.BootColor;
        }
    }
}