using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Models;


public class SceneLoader : MonoBehaviour
{
    private Button button;

	void Start () {
		button = this.gameObject.GetComponent<Button>();
		button.onClick.AddListener(LoadNextScene);
	}

	void LoadNextScene(){
		Debug.Log ("Loading next scene");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}
}
