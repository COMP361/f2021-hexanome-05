using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossButtonManager : MonoBehaviour
{
	public GameObject button;
	public GameObject HelpMenu;

	public void OnMouseDown()
	{
		HelpMenu.SetActive(false);
	}
}
