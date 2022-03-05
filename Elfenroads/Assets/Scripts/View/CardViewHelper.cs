using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Controls;

public class CardViewHelper : MonoBehaviour,  IPointerClickHandler
{
    public MoveBootController moveBootController; //***Could use a cleanup / merge with CounterViewHelper, or a way to specify this object.
    private Guid myGuid;

    void Start(){
        moveBootController = GameObject.Find("MoveBootController").GetComponent<MoveBootController>();
    }

    public void OnPointerClick(PointerEventData eventData){
        moveBootController.CardClicked(this.gameObject);
    }

    public void setGuid(Guid input){
        myGuid = input;
    }

    public Guid getGuid(){
        return myGuid;
    }

}