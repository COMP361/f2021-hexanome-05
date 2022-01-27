using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firesplash.UnityAssets.SocketIO;
using Newtonsoft.Json.Linq;

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
    private SocketIOCommunicator socket;
    
    // $#Y+qDctAF3Fvk?@
    public void beginChooseColors(SessionInfo sI, SocketIOCommunicator inputSocket){
        socket = inputSocket;
        socket.Instance.On("ColorChosen", updateColors); 
        canvas.SetActive(true);
        playerName = sI.getClient().clientCredentials.username;
        sessionID = sI.getClient().thisSessionID;
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
        Debug.Log(input);
        //Need some way to prevent a person who has chosen a color to choose any remaining colors (Server-side, right?)
    }

    public void chooseRed(){
        Debug.Log("red chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Red");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }

    public void chooseBlue(){  
        Debug.Log("blue chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Blue");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }

    public void chooseGreen(){
        Debug.Log("green chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Green");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }

    public void chooseYellow(){
        Debug.Log("yellow chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Yellow");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }

    public void choosePurple(){
        Debug.Log("purple chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Purple");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }

    public void chooseBlack(){
        Debug.Log("black chosen!");
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Black");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
    }
}
