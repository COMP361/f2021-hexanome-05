using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Models;

public class Json : MonoBehaviour
{
    void Start() {
        Road road = new Road(new Town("hottown"), new Town("cooltown"), RoadType.Plain);
        Debug.Log(JsonConvert.SerializeObject(road));
    }
}
