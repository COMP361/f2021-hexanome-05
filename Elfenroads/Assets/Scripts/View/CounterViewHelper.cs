using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CounterViewHelper : MonoBehaviour,  IPointerClickHandler
{
    public GameObject faceUpCountersView;
    public Guid myGuid;

    void Start(){
        faceUpCountersView = GameObject.Find("FaceUpCounters");
    }

    public void OnPointerClick(PointerEventData eventData){
        faceUpCountersView.GetComponent<FaceUpCountersView>().CounterClicked(this.gameObject);
    }
}
