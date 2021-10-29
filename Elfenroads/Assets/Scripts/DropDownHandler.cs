using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        List<string> items = new List<string>() { "","Chris' saved game 1" , "Elfenroads saved file #2"};
        
        foreach(var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }
      //  DropdownItemSelected(dropdown);
     //   dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });

    }
    /*
    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        TextBox.text = dropdown.options[index].text;
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
