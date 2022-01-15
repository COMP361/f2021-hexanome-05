using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System.Text.Json;

public class TestModelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string json = @"{""players"": [
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
    Debug.Log(json);

    Player p = JsonSerializer.Deserialize<Player>(json);
    Debug.Log(json);
    Debug.Log(p.name);

    }
}
