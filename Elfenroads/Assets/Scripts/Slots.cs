using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour{
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

    public void addToSlot(GameObject prefab){
        foreach(Slot s in slots){
            if(s.obj == null){
                GameObject newPiece = Instantiate(prefab, new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f), Quaternion.identity); //Hopefully this works!
                s.obj = newPiece;
                return;
            }
        }
        Debug.Log("No slot found!");
    }

    public void removeFromSlot(GameObject obj){ 
        foreach(Slot s in slots){
            if(s.obj == obj){
                Destroy(s.obj);
                s.obj = null;
                return;
            }
        }
    }

    public void removeAllFromSlots(){
        foreach (Slot s in slots){
            if(s.obj != null){
                Destroy(s.obj);
                s.obj = null;
            }
        }
    }



}