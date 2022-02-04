using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class FaceUpCounters
    {
        public List<Card> counters;

        public FaceUpCounters(){
            counters = new List<Card>(5);
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.
    }
}