using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camSpeed;
    public float zoomSpeed;
    public float limitX;
    public float limitY;
    public float maxZoom;

    private float minZoom = 16f;

    void Start(){
        Camera.main.orthographicSize = minZoom;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if(Input.GetKey("w")){
            newPos.y += camSpeed * Time.deltaTime;
        }
        if(Input.GetKey("s")){
            newPos.y -= camSpeed * Time.deltaTime;
        }
        if(Input.GetKey("a")){
            newPos.x -= camSpeed * Time.deltaTime;
        }
        if(Input.GetKey("d")){
            newPos.x += camSpeed * Time.deltaTime;
        }

        newPos.x = Mathf.Clamp(newPos.x, -limitX, limitX);
        newPos.y = Mathf.Clamp(newPos.y, -limitY, limitY);
        transform.position = newPos;
    }
}
