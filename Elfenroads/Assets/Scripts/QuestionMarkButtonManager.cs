using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkButtonManager : MonoBehaviour
{
	public GameObject button;
	public GameObject HelpMenu;

	void Start()
	{
		HelpMenu.SetActive(false);
	}

	public void OnMouseDown()
	{
		HelpMenu.SetActive(true);
	}
}
