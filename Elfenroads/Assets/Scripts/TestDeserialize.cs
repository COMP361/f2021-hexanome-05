using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Models;

public class TestDeserialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string playerListJson = @"{""players"":[{""name"":""maex"",""boot"":{""color"":""Red"",""id"":""b1e98436-0b87-877b-d64d-4c531de4b4ab""},""inventory"":{""townPieces"":[]}},{""name"":""ryan"",""boot"":{""color"":""Blue"",""id"":""672bd4a5-5589-279e-7555-cf38d739cd30""},""inventory"":{""townPieces"":[]}}]}";
        JObject jobj = JObject.Parse(playerListJson);
        JArray jarr = JArray.Parse(jobj["players"].ToString());

        //TESTING DESERIALIZATION USING JSONCONVERT, WHILE ALSO USING JARRAY TO FLIP THROUGH THEM. - WORKING!
        var playerVar = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(jarr[0].ToString());
        Debug.Log(playerVar.name);
        Debug.Log(playerVar.boot.id);

        //TESTING DESERIALIZATION OF A LIST OF AN ABSTRACT CLASS - WORKING!
        List<Counter> counters = new List<Counter>();
        Guid g1 = Guid.NewGuid();
        Guid g2 = Guid.NewGuid();
        Debug.Log(g1 + ", " + g2);

        counters.Add(new TransportationCounter(g1, CardType.Troll));
        counters.Add(new MagicSpellCounter(g2, SpellType.Double));
        var jset = new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects }; //This thing is crucial! Must be passed in to both the Serilization and the Deserialization function!
        string serializedCounters = Newtonsoft.Json.JsonConvert.SerializeObject(counters, jset);
        Debug.Log(serializedCounters);                                                             //Looks like: [{"$type":"Models.TransportationCounter, Elfenroads","cardType":4,"id":12},{"$type":"Models.MagicSpellCounter, Elfenroads","spellType":1,"id":14}]
        List<Counter> deserializedCounters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Counter>>(serializedCounters, jset);
        Debug.Log(deserializedCounters[0] is TransportationCounter);
        Debug.Log(deserializedCounters[1] is MagicSpellCounter);
    }

}
