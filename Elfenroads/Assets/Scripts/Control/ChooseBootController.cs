using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firesplash.UnityAssets.SocketIO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Models;

public class ChooseBootController : MonoBehaviour
{
    public GameObject canvas;
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button blackButton;
    public Button yellowButton;
    public Button purpleButton;
    public TMPro.TMP_Text textBox;

    private ColorBlock defaultBlock;
    private string playerName;
    private int numPlayers;
    private string sessionID; 
    private string chosenColor;
    private SocketIOCommunicator socket;
    
    // $#Y+qDctAF3Fvk?@
    public void beginChooseColors(SessionInfo sI, SocketIOCommunicator inputSocket){
        socket = inputSocket;
        socket.Instance.On("ColorChosen", updateColors); 
        canvas.SetActive(true);
        playerName = sI.getClient().clientCredentials.username;
        numPlayers = sI.getClient().mySession.players.Count;
        Debug.Log("Number of players according to ChooseBoot: " + numPlayers);
        sessionID = sI.getClient().thisSessionID;
        socket.Instance.On("GameState", updateTest); 
        defaultBlock = redButton.colors; //Save for later.
    }

    //Either calls ElfenroadsControl, or will be called by ElfenroadsControl.
    public void endChooseColors(){
        Debug.Log("Reached endChooseColors!");
        socket.Instance.Off("ColorChosen");
        canvas.SetActive(false);
        //Calls ElfenroadsControl here.
    }

    public void updateTest(string input){
        Debug.Log(input);
        var jset = new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects, MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead, ReferenceLoopHandling = ReferenceLoopHandling.Serialize };
        Game initialGame = Newtonsoft.Json.JsonConvert.DeserializeObject<Game>(input, jset);
        Debug.Log(initialGame.startingPlayer.name);
        //JObject jobj = JObject.Parse(input);
        //JArray counterArr = JArray.Parse(jobj["counters"].ToString());
        //Debug.Log(counterArr);
        //List<Counter> counterList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Counter>>(counterArr.ToString(), jset);
        // Debug.Log(counterList[0].GetType());
        // Debug.Log("Should be true: " + (counterList[0] is TransportationCounter));
        // Debug.Log("Fields: Id: " + counterList[0].id + ", Type: " + ((TransportationCounter) counterList[0]).transportType);
        // Debug.Log("Should be true: " + (counterList[1] is MagicSpellCounter));
        // Debug.Log("Fields: Id: " + counterList[1].id + ", Type: " + ((MagicSpellCounter) counterList[1]).spellType);
    }

    public void updateColors(string input){
        Debug.Log("reached updateColors"); 
        Debug.Log(input);
        JObject jobj = JObject.Parse(input);
        JArray jarr = JArray.Parse(jobj["players"].ToString());
        Player curPlayer = null;
        List<Player> players = new List<Player>();
        for(int i = 0 ; i < jarr.Count ; i++){
            Debug.Log("Loop " + i + "!");
            curPlayer = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(jarr[i].ToString());
            
            //First, if curPlayer's name is the same name as the Client recieving it, then we just leave all buttons disabled and return.
            if(curPlayer.name == playerName){
                return;
            }else{
                players.Add(curPlayer);
            }
        }

        //Else, we'll enable all buttons again, but disable colors as we find them.
        enableAllButtons();
        foreach(Player p in players){
            Models.Color curColor = p.boot.color;

            switch(curColor){
                case Models.Color.RED:
                {
                    disableButton(redButton);
                    break;
                }
                case Models.Color.BLUE:
                {
                    disableButton(blueButton);
                    break;
                }
                case Models.Color.GREEN:
                {
                    disableButton(greenButton);
                    break;
                }
                case Models.Color.YELLOW:
                {
                    disableButton(yellowButton);
                    break;
                }
                case Models.Color.BLACK:
                {
                    disableButton(blackButton);
                    break;
                }
                case Models.Color.PURPLE:
                {
                    disableButton(purpleButton);
                    break;
                }
                default: break; //Throw error here? Should never be reached though. ***
            }
        }
        textBox.text = "Choose your Boot!";
        textBox.fontSize = 36;

    
    }

    public void chooseRed(){
        Debug.Log("red chosen!");
        textBox.text = "You have chosen red. Waiting for other players...";
        textBox.fontSize = 20;
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
        textBox.text = "You have chosen blue. Waiting for other players...";
        textBox.fontSize = 20;
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
        textBox.text = "You have chosen green. Waiting for other players...";
        textBox.fontSize = 20;
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
        textBox.text = "You have chosen yellow. Waiting for other players...";
        textBox.fontSize = 20;
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
        textBox.text = "You have chosen purple. Waiting for other players...";
        textBox.fontSize = 20;
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
        textBox.text = "You have chosen black. Waiting for other players...";
        textBox.fontSize = 20;
        JObject json = new JObject();
        json.Add("game_id", sessionID);
        json.Add("name", playerName);
        json.Add("color", "Black");
        socket.Instance.Emit("ChooseColor", json.ToString(), false);
        chosenColor = "Black";
        disableAllButtons();
    }

    public void disableButton(Button b){
        b.enabled = false;
        ColorBlock cb = new ColorBlock();
        cb.disabledColor = defaultBlock.disabledColor;
        cb.normalColor = defaultBlock.disabledColor;
        cb.highlightedColor = defaultBlock.disabledColor;
        b.colors = cb;
    }

    public void enableAllButtons(){
        redButton.enabled = true;
        blueButton.enabled = true;
        greenButton.enabled = true;
        yellowButton.enabled = true;
        blackButton.enabled = true;
        purpleButton.enabled = true;

        redButton.colors = defaultBlock;
        blueButton.colors = defaultBlock;
        greenButton.colors = defaultBlock;
        yellowButton.colors = defaultBlock;
        blackButton.colors = defaultBlock;
        purpleButton.colors = defaultBlock;
    }

    public void disableAllButtons(){
        redButton.enabled = false;
        blueButton.enabled = false;
        greenButton.enabled = false;
        yellowButton.enabled = false;
        blackButton.enabled = false;
        purpleButton.enabled = false;

        ColorBlock cb = new ColorBlock();
        cb.disabledColor = defaultBlock.disabledColor;
        cb.normalColor = defaultBlock.disabledColor;
        cb.highlightedColor = defaultBlock.disabledColor;

        redButton.colors = cb;
        blueButton.colors = cb;
        greenButton.colors = cb;
        yellowButton.colors = cb;
        blackButton.colors = cb;
        purpleButton.colors = cb;
    }
}
