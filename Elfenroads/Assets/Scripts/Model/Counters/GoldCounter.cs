using System.Collections;
using System.Collections.Generic;
using Models;
using System;

namespace Models
{
    public class GoldCounter : Counter
    {
        public GoldCounter(Guid id){
            this.id = id;
        }
    }
}