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

    public void setCurrentCity(GameObject city){
        currentCity = city;
    }

    public GameObject getCurrentCity(){
        return currentCity;
    }

    public void setColor(BootColor pColor){
        aColor = pColor;
    }

    public BootColor getColor(){
        return aColor;
    }
}
