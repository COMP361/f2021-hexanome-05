using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    private bool dragging;
    public GameObject Grid;
    public GameObject Canvas;
    public Vector3 originalPosition;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = originalPosition;
        Grid.SetActive(true);
        Canvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(position);
        }
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
