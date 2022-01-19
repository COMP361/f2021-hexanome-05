using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Newtonsoft.Json;

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

      List<TownPiece> townPieces = new List<TownPiece>();
      TownPiece townPiece = new TownPiece(23, Models.Color.RED);
      Town town = new Town("ChrisTown");
      ModelHelper.StoreInstance().addTown(town);
      ModelHelper.StoreInstance().addTownPiece(townPiece);

      Town townUpdate = JsonConvert.DeserializeObject<Town>(townJson);

      Debug.Log("SRIALIZE TEST");
      Debug.Log(JsonConvert.SerializeObject(townUpdate, Formatting.Indented));

      Debug.Log("DESERIALIZE TEST");
      Debug.Log(townUpdate.TESTdebug());

      Debug.Log("UPDATE TEST");
      town.Update(townUpdate);
      Debug.Log(town.TESTdebug());

      Debug.Log("STORE TEST");
      Debug.Log(ModelHelper.StoreInstance().getTown("ChrisTown").TESTdebug());

    }
}
