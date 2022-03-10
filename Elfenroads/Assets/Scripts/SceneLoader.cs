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
		GameObject.Find("SessionInfo").GetComponent<SessionInfo>().getClient().socket.Instance.Close(); ///*** PLEASE
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
