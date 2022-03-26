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

        //TransportationCounter tc = new TransportationCounter(g1, TransportType.TrollWagon);
        // counters.Add(tc);
        // counters.Add(new MagicSpellCounter(g2, SpellType.Double));
        var jset = new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects, MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead, ReferenceLoopHandling = ReferenceLoopHandling.Serialize }; //This thing is crucial! Must be passed in to both the Serilization and the Deserialization function!
        // string serializedCounters = Newtonsoft.Json.JsonConvert.SerializeObject(counters, jset);
        // Debug.Log(serializedCounters);    
        // string serializedCounter = Newtonsoft.Json.JsonConvert.SerializeObject(tc, jset); 
        // Debug.Log(serializedCounter);
        // Counter deserializedCounter = Newtonsoft.Json.JsonConvert.DeserializeObject<Counter>(serializedCounter, jset);                                           
        // // List<Counter> deserializedCounters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Counter>>(serializedCounters, jset);
        // // Debug.Log(deserializedCounters[0] is TransportationCounter);
        // // Debug.Log(deserializedCounters[1] is MagicSpellCounter);
        // Debug.Log(deserializedCounter);

        // string json = @"{""$type"":""Models.TransportationCounter, Elfenroads"", ""transportType"":2, ""id"":""768113a1-ea8e-7a13-5b16-0488fd56187f""}";
        // Debug.Log("Our string: " + json);
        // Counter countersTest = Newtonsoft.Json.JsonConvert.DeserializeObject<Counter>(json, jset);
        // Player mainPlayer = new Player("test", Models.Color.RED);
        // Player secondPlayer = new Player("test2", Models.Color.BLUE);
        // List<Player> players = new List<Player>();
        // players.Add(mainPlayer);
        // players.Add(secondPlayer);
        // Board board = new Board(new List<Road>(), new List<Town>());
        // //GamePhase countersPhase = new DrawCounters(mainPlayer);
        // Game testGame = new Game(board, players, mainPlayer, Game.Variant.Elfenland, 1);
        // testGame.currentPhase = countersPhase;
        // string serializedGame = Newtonsoft.Json.JsonConvert.SerializeObject(testGame, jset);
        // Debug.Log(serializedGame);
        // Card testCard = new TravelCard(TransportType.Dragon);
        // string serializedCard = Newtonsoft.Json.JsonConvert.SerializeObject(testCard, jset);
        // Debug.Log(serializedCard);
    }

}
