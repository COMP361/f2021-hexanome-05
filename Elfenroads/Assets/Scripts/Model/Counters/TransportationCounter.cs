using System.Collections;
using System.Collections.Generic;
using Models;
using System;
using Newtonsoft.Json;

namespace Models
{
    public class TransportationCounter : Counter
    {
        public CardType cardType { set; get; }

        public TransportationCounter(Guid id, CardType cardType){
            this.id = id;
            this.cardType = cardType;
        }
    }

    public enum CardType{
        Pig,
        Cycle,
        Cloud,
        Unicorn,
        Troll,
        Dragon
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