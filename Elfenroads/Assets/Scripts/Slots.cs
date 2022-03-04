using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots {
    public List<Slot> slots;

    public Slots(int maxItems, int itemsPerRow, Vector3 initialPosition, float xBuffer, float yBuffer){
        float colCount = 0;
        float rowCount = 0;
        this.slots = new List<Slot>();
        for(int i = 0 ; i < maxItems ; i++){
            slots.Add(new Slot(initialPosition + new Vector3(xBuffer * colCount, - (yBuffer * rowCount), 0f)));
            colCount = (colCount + 1) % itemsPerRow;
            if(colCount == 0) rowCount++;
        }
    }

    public void addToSlot(GameObject instantiatedObject, GameObject inputObject){
        foreach(Slot s in slots){
            if(s.obj == null){
                instantiatedObject.transform.position = new Vector3(s.xCoord, s.yCoord, inputObject.transform.position.z + 0.5f);
                s.obj = instantiatedObject;
                return;
            }
        }
        Debug.Log("No slot found!");
    }

    public List<Slot> getSlots(){
        return slots;
    }



}