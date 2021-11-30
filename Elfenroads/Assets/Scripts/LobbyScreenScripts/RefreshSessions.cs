using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Refresh : MonoBehaviour
{
    private Button refreshButton;

    public GameObject openSessions;

    // Start is called before the first frame update
    void Start(){
        //Retrieve the button for Refresh.
        refreshButton = this.gameObject.GetComponent<Button>();

        //Onclick listener for "refreshing".
        refreshButton.onClick.AddListener(Update);
    }

    // Update is called once per frame
    void Update(){
        var listElements = new List<string>{"Name", "Creator", "Players"};
        foreach (var elements in listElements)
        {
            //AssetDatabase is to be replaced with the actual Database
            //AssetDatabase.Refresh();
        }
    }
}
