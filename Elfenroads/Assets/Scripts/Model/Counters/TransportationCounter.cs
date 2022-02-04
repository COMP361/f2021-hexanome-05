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

    public class TCConverter : JsonConverter{
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}