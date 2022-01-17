using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot{
    public float xCoord { set; get; }
    public float yCoord { set; get; }
    public GameObject obj { set; get; }

    public Slot(Vector3 inputPos){
        xCoord = inputPos.x;
        yCoord = inputPos.y;
    }

}

public class SlotTester : MonoBehaviour
{

    private List<Slot> slots;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 initialSlot = gameObject.transform.position - new Vector3(0.25f, 0f, 0f);
        float colCount = 0;
        float rowCount = 0;
        slots = new List<Slot>();
        for(int i = 0 ; i < 6 ; i++){
            slots.Add(new Slot(initialSlot + new Vector3(0.25f * colCount, - (0.25f * rowCount), 0f)));
            colCount = (colCount + 1) % 3;
            if(colCount == 0) rowCount++;
        }
        foreach(Slot s in slots){
            Debug.Log("Slot has positions x: " + s.xCoord + " and y: " + s.yCoord);
        }

    }

    public void addToSlot(GameObject obj){

        foreach(Slot s in slots){
            if(s.obj == obj){
                Debug.Log("Boot is already in a slot!");
                return;
            }
        }

        foreach(Slot s in slots){
            if(s.obj == null){
                s.obj = obj;
                obj.transform.position = (new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f));
                return;
            }else{
            }
        }
    }

    public void removeFromSlot(GameObject obj){ //What to do with an object when it is removed from a slot?  -> It would be added somewhere else (by necessity) so just remove it from the slot array.
        foreach(Slot s in slots){
            if(s.obj == obj){
                s.obj = null;
                return;
            }else{
                Debug.Log("Nothing to remove!");
            }
        }
    }
}
