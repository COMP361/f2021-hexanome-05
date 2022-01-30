using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Newtonsoft.Json;

#pragma warning disable 0219
public class TestModelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      string playerJson = @"{""players"": [
      {
        ""name"": ""Chris"",
        ""color"": 1,
        ""boot"": {
          ""color"": 1,
          ""currentTown"": {
            ""townName"": ""1"",
            ""townPieces"": [
              {
                ""color"": 1
              }
            ]
          }
        },
        ""inventory"": {
          ""townPieces"": [
            {
              ""color"": 1
            }
          ]
        }
      }
      ]}";

      string townJson = @"{
        ""name"": ""ChrisTown"",
        ""townPieces"": [
          {
            ""id"": 23,
            ""color"": 1
          }
        ]}";

      string counterJson = @"{
        ""id"": 1234,
        }";
      
      string transportationCounterJson = @"{
        ""id"": 1234,
        ""
        }";

      List<Counter> list = new List<Counter>();
      Counter transCounter = new TransportationCounter(10, CardType.Cloud);
      Counter obsCounter = new ObstacleCounter(14, ObstacleType.Land);
      // JsonConvert.DeserializeObject<Counter>(counterJson);
      //TransportationCounter transportationCounter = JsonConvert.DeserializeObject<TransportationCounter>(transportationCounterJson);


      list.Add(transCounter);
      list.Add(obsCounter);
      string serialized = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.Auto
      });
      Debug.Log(serialized);

      List<Counter> counterList = JsonConvert.DeserializeObject<List<Counter>>(serialized, new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.Auto
      });
      
      Debug.Log(JsonConvert.SerializeObject(counterList, Formatting.Indented, new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.Auto
      }));
      // Debug.Log("Counter ID: " + counter.id);
      // Debug.Log("TransportationCounter ID: " + transportationCounter.id + ", Type: " + transportationCounter.);

      // List<TownPiece> townPieces = new List<TownPiece>();
      // TownPiece townPiece = new TownPiece(23, Models.Color.RED);
      // Town town = new Town("ChrisTown");
      // ModelHelper.StoreInstance().addTown(town);
      // ModelHelper.StoreInstance().addTownPiece(townPiece);

      // Town townUpdate = JsonConvert.DeserializeObject<Town>(townJson);

      // Debug.Log("SRIALIZE TEST");
      // Debug.Log(JsonConvert.SerializeObject(townUpdate, Formatting.Indented));

      // Debug.Log("DESERIALIZE TEST");
      // Debug.Log(townUpdate.TESTdebug());

      // Debug.Log("UPDATE TEST");
      // town.Update(townUpdate);
      // Debug.Log(town.TESTdebug());

      // Debug.Log("STORE TEST");
      // Debug.Log(ModelHelper.StoreInstance().getTown("ChrisTown").TESTdebug());

    }
}
#pragma warning restore 0219