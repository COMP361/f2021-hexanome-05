using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotRemoveTest : MonoBehaviour
{

    public GameObject boot;
    public GameObject town;
    public Button thisButton;
    public Transform removedPos;

    private Vector3 vRemovedPos;

    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(removeBoot);
        vRemovedPos = removedPos.transform.position;
    }
    
    public void removeBoot(){
        town.GetComponent<SlotTester>().removeFromSlot(boot);
        boot.transform.position = vRemovedPos;
    }
}
