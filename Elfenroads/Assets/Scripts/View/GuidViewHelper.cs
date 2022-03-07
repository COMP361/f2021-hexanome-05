using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

public class GuidViewHelper : MonoBehaviour,  IPointerClickHandler
{
    private GuidHelperContainer controller; 
    private Guid myGuid;

    public void OnPointerClick(PointerEventData eventData){
        controller.GUIClicked(this.gameObject);
    }

    public void setContainer(GuidHelperContainer c){
        Debug.Log("Container set!");
        controller = c;
    }

    public void setGuid(Guid input){
        myGuid = input;
    }

    public Guid getGuid(){
        return myGuid;
    }

}