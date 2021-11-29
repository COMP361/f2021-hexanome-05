using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossButtonManager : MonoBehaviour
{
	public GameObject button;
	public GameObject grid;

	public void OnMouseDown()
	{
		grid.SetActive(false);
	}
}
