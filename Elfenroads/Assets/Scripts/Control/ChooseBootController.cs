using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firesplash.UnityAssets.SocketIO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class ChooseBootController : MonoBehaviour
{
    public GameObject canvas;
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button blackButton;
    public Button yellowButton;
    public Button purpleButton;

    private string playerName;
    private string sessionID; 
    private string chosenColor;
    private SocketIOCommunicator socket;
    
    // $#Y+qDctAF3Fvk?@
    public void beginChooseColors(SessionInfo sI, SocketIOCommunicator inputSocket){
        socket = inputSocket;
        socket.Instance.On("ColorChosen", updateColors); 
        canvas.SetActive(true);
        playerName = sI.getClient().clientCredentials.username;
        sessionID = sI.getClient().thisSessionID;
        socket.Instance.On("GameState", updateTest); 
    }

    //Either calls ElfenroadsControl, or will be called by ElfenroadsControl.
    public void endChooseColors(){
        socket.Instance.Off("ColorChosen");
        canvas.SetActive(false);
        //Calls ElfenroadsControl here.
    }

    public void updateColors(string input){
        //Input will be a list of colors. Turn this into a list object, and turn off buttons which are of the colors that are present in the list.
        // ["Blue", "Green", "Red"]
        Debug.Log("reached updateColors"); 
        //var jsonString = input.Replace('\"', '"');
        Debug.Log(input);
        // string jsonString = Regex.Unescape(input);
        // jsonString = jsonString.Remove(jsonString.Length - 1, 1).Remove(0,1); //remove first and last qoutes
        // Debug.Log(jsonString);
        // JObject jsonObj = JObject.Parse(jsonString);
        // Debug.Log(jsonObj.ToString());
        // Debug.Log(jsonObj[0]["name"]);
        JObject jsonObj;
        // try{
        //     //JToken token = JToken.Parse(input);
        //     jsonObj = JObject.Parse(input);
        //     Debug.Log("Json Object in 1st block:" + jsonObj.ToString());
        // }catch (JsonException e){
        //     Debug.Log(e);
        // }

        try{
            //string jsonString = Regex.Unescape(input);
            // jsonString = jsonString.Remove(jsonString.Length - 1, 1).Remove(0,1); //remove first and last qoutes
            //Debug.Log(jsonString);
            jsonObj = JObject.Parse(input);
            Debug.Log("Json Object in 2nd block:" + jsonObj.ToString());
            Debug.Log(jsonObj["players"]);
        }catch (JsonException e){
            Debug.Log(e);
        }

        // JToken token = JToken.Parse(input);
        // JObject jsonObj = JObject.Parse((string) token);
        // Debug.Log(jsonObj.ToString());
        //[{\"name\":\"maex\",\"bootColor\":\"Red\",\"inventory\":{\"townsCollected\":[]}}]
        
        //Now, we need to parse the recieved list and disable the appropriate buttons.
        
        //Finally, need to make a check with "chosenColor". If it was in the recieved list, we don't re-enable the buttons.
        //Otherwise, if it is null or it is not in the recieved list, we re-enable buttons. (may not be enough, may need Server-side);
    }

    public void updateTest(string input){
        Debug.Log(input);
    }


    public void chooseRed(){
        Debug.Log("red chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Red");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Red";
        disableAllButtons();
    }

    public void chooseBlue(){  
        Debug.Log("blue chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Blue");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Blue";
        disableAllButtons();
    }

    public void chooseGreen(){
        Debug.Log("green chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Green");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Green";
        disableAllButtons();
    }

    public void chooseYellow(){
        Debug.Log("yellow chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Yellow");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Yellow";
        disableAllButtons();
    }

    public void choosePurple(){
        Debug.Log("purple chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Purple");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Purple";
        disableAllButtons();
    }

    public void chooseBlack(){
        Debug.Log("black chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Black");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Black";
        disableAllButtons();
    }

    public void disableAllButtons(){
        redButton.enabled = false;
        blueButton.enabled = false;
        greenButton.enabled = false;
        yellowButton.enabled = false;
        blackButton.enabled = false;
        purpleButton.enabled = false;
    }
}
