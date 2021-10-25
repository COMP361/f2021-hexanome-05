using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BootScript : MonoBehaviour
{
    private GameObject currentCity;

    public void setCurrentCity(GameObject city){
        currentCity = city;
    }

    public GameObject getCurrentCity(){
        return currentCity;
    }
}
