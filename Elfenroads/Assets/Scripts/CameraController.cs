using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camSpeed;
    public float zoomSpeed;
    public float limitX;
    public float limitY;
    public float scrollSpeed;

    public float minZoom = 10f;
    public float maxZoom = 16f;

    void Start(){
        Camera.main.orthographicSize = 14f;
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
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //Move the camera up/down/left/right
        newPos.x = Mathf.Clamp(newPos.x, -limitX, limitX);
        newPos.y = Mathf.Clamp(newPos.y, -limitY, limitY);

        //Adjust zoom
        float orthoSizeCur = Camera.main.orthographicSize;
        orthoSizeCur += (-scroll) * scrollSpeed * Time.deltaTime;
        orthoSizeCur = Mathf.Clamp(orthoSizeCur, minZoom, maxZoom);
        Camera.main.orthographicSize = orthoSizeCur;

        transform.position = newPos;
    }
}
