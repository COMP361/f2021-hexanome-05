using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitScript : MonoBehaviour
{
    private Button button;

	void Start () {
		button = this.gameObject.GetComponent<Button>();
		button.onClick.AddListener(Quit);
	}

    // Update is called once per frame
    void Quit(){
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
