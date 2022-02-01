using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Models;

public class TestDeserialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string playerListJson = @"{""players"":[{""name"":""maex"",""boot"":{""color"":""Red"",""id"":""b1e98436-0b87-877b-d64d-4c531de4b4ab""},""inventory"":{""townPieces"":[]}},{""name"":""ryan"",""boot"":{""color"":""Blue"",""id"":""672bd4a5-5589-279e-7555-cf38d739cd30""},""inventory"":{""townPieces"":[]}}]}";
        JObject jobj = JObject.Parse(playerListJson);
        JArray jarr = JArray.Parse(jobj["players"].ToString());
        Debug.Log(jarr);
        Player player = jarr[0].ToObject<Player>();
        Debug.Log(player.boot.color);
        Debug.Log(player.boot.id);
        //Boot boot = jarr[0]["boot"].ToObject<Boot>();
        //Debug.Log(boot.id);
        //var player = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(jsonObj["players"][0].ToString());
        //Debug.Log(player.name);
        //var player = jsonObj["players"][0].ToObject<Player>();
        //Debug.Log(player.name);
        

    }

}
