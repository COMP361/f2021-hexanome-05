using System.Collections;
using System.Collections.Generic;
using Models;
using System;
using Newtonsoft.Json;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public TransportType transportType { set; get; }

        public TransportationCounter(Guid id, TransportType transportType){
            this.id = id;
            this.transportType = transportType;
        }
    }

}