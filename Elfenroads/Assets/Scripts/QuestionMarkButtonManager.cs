using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class QuestionMarkButtonManager : MonoBehaviour {
	public GameObject button;
	public GameObject grid;

	void Start() {
		grid.SetActive(false);
	}

	public void OnMouseDown() {
		grid.SetActive(true);
	}
}
