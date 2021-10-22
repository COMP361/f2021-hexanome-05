using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoot : MonoBehaviour
{
    // StartTownPosition = where the boot at the beginning of the game
    public Vector2 StartTownPosition = new Vector2(0.0f, 0.0f);

    // indicates if we are in the MoveBoot phase
    private bool isMoving = true;


    void Start()
    {
        transform.position = (StartTownPosition);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMoving)
        {
            // boot got "picked up" by the mouse andfollows the mouse 
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = (pos);
        }
    }
}
