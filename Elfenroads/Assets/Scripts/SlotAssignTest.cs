using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotAssignTest : MonoBehaviour
{
    public GameObject boot;
    public GameObject town;
    public Button thisButton;

    void Start()
    {
        thisButton.onClick.AddListener(addBoot);
    }
    
    public void addBoot(){
        town.GetComponent<SlotTester>().addToSlot(boot);
    }
}
