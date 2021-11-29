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
    public BootColor color;
    private GameObject aInventory;

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
}
