using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CounterViewHelper : MonoBehaviour,  IPointerClickHandler
{
    public GameObject faceUpCountersView;
    private Guid myGuid;

    void Start(){
        faceUpCountersView = GameObject.Find("FaceUpCounters");
    }

    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("Counter with ID " + this.myGuid + " was clicked!");
        faceUpCountersView.GetComponent<FaceUpCountersView>().CounterClicked(this.gameObject);
    }

    public void setGuid(Guid input){
        myGuid = input;
    }

    public Guid getGuid(){
        return myGuid;
    }

}
