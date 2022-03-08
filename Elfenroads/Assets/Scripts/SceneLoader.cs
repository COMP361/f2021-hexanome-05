using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Models;


public class SceneLoader : MonoBehaviour
{
	public string sceneName;
	
	public void loadScene(){
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
