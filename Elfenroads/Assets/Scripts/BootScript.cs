using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private GameObject inventory;
    public GameObject townPieceCounter;

    public void setTownPieceCounter(GameObject tCounter){
        this.townPieceCounter = tCounter;
    }

    public void setCurrentCity(GameObject city){
        currentCity = city;
    }

    public GameObject getCurrentCity(){
        return currentCity;
    }

    public void setInventory(GameObject inv){
        this.inventory = inv;
    }

    public GameObject getInventory(){
        return this.inventory;
    }

    void Update(){
        townPieceCounter.GetComponent<TextMeshProUGUI>().SetText(inventory.GetComponent<InventoriesManager>().countTownPiece().ToString());
    }


}
