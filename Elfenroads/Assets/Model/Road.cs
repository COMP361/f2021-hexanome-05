using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;



namespace Models
{
    public abstract class Road
    {
        public Town Start { get; set; }
        public Town End { get; set; }
       // public List<string> Counters { get; set; }

        public Road(Town start, Town end)
        {
            this.Start = start;
            this.End = end;
        }

        public Town getStart()
        {
            return this.Start;
        }
        
        public Town getEnd()
        {
            return this.End;
        }

        public void onClick()
        {
            //This road was clicked. Inform the MoveBootsManager, who will verify that it was a valid road to click on for movement.
         
           // MoveBootsManager.instance.roadClicked(gameObject);
        }
    }

}