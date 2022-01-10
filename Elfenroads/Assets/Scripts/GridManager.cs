using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class GridManager : MonoBehaviour
{
    private bool dragging;
    public GameObject Grid;
    public Vector3 originalPosition;

    void Update()
    {
        if (dragging)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(position);
        }
    }

    void Start()
    {
        Grid.transform.position = originalPosition;
    }

    public void OnMouseDown()
    {
        dragging = true;
    }

    public void OnMouseUp()
    {
        dragging = false;
    }


}
