using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BootColor{
    RED,
    BLUE
}

public class BootScript : MonoBehaviour
{
    private GameObject currentCity;
    public Vector3 Offset;
    public Sprite blueSprite;
    private BootColor aColor;
    private GameObject aInventory;
    private string playerName;

    public void setCurrentCity(GameObject city){
        currentCity = city;
    }

    public GameObject getCurrentCity(){
        return currentCity;
    }

    public void setInventory(GameObject inv){
        aInventory = inv;
    }

    public GameObject getInventory(){
        return aInventory;
    }

    public void setName(string name){
        playerName = name;
    }

    public string getname(){
        return playerName;
    }

    public void setColor(BootColor pColor){
        aColor = pColor;
    }

    public BootColor getColor(){
        return aColor;
    }
}
