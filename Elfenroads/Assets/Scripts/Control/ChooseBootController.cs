using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firesplash.UnityAssets.SocketIO;

public class ChooseBootController : MonoBehaviour
{
    public GameObject canvas;
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button blackButton;
    public Button yellowButton;
    public Button purpleButton;

    private SocketIOCommunicator socket;
    

    public void beginChooseColors(SocketIOCommunicator inputSocket){
        socket = inputSocket;
        socket.Instance.On("ChooseColor", updateColors); 
        canvas.SetActive(true);
    }

    //Either calls ElfenroadsControl, or will be called by ElfenroadsControl.
    public void endChooseColors(){
        socket.Instance.Off("ChooseColor");
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
        socket.Instance.Emit("ChooseColor", "Red", true);
    }

    public void chooseBlue(){  
        Debug.Log("blue chosen!");
        socket.Instance.Emit("ChooseColor", "Blue", true);
    }

    public void chooseGreen(){
        Debug.Log("green chosen!");
        socket.Instance.Emit("ChooseColor", "Green", true);
    }

    public void chooseYellow(){
        Debug.Log("yellow chosen!");
        socket.Instance.Emit("ChooseColor", "Yellow", true);
    }

    public void choosePurple(){
        Debug.Log("purple chosen!");
        socket.Instance.Emit("ChooseColor", "Purple", true);
    }

    public void chooseBlack(){
        Debug.Log("black chosen!");
        socket.Instance.Emit("ChooseColor", "Black", true);
    }
}
