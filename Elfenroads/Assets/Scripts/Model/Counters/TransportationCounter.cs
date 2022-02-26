using System.Collections;
using System.Collections.Generic;
using Models;
using System;
using Newtonsoft.Json;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public TransportType cardType { protected set; get; }

        public TransportationCounter(TransportType  cardType) : base() {
            this.cardType = cardType;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected TransportationCounter(TransportType cardType, Guid id) : base(id) {
            this.cardType = cardType;
        }
    }

}