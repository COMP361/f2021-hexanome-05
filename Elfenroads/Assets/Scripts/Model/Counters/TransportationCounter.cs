using System.Collections;
using System.Collections.Generic;
using Models;
using System;
using Newtonsoft.Json;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public TransportType transportType { protected set; get; }

        public TransportationCounter(TransportType transportType) : base() {
            this.transportType = transportType;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected TransportationCounter(TransportType transportType, Guid id) : base(id) {
            this.transportType = transportType;
        }
    }

}