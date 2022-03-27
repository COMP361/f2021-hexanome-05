using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camSpeed;
    // public float zoomSpeed;
    // public float limitX;
    // public float limitY;
    public float scrollSpeed;

    public float smoothingFactor = 8f;

    // public float minZoom = 8f;
    // public float maxZoom = 16f;
    public bool locked = true;
    private float targetZoom = 16f;

    void Start(){
        Camera.main.orthographicSize = 16f;
        Elfenroads.Control.LockCamera += lockCamera;
        Elfenroads.Control.UnlockCamera += unlockCamera;

        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    void unlockCamera(object sender, EventArgs e){
        locked = false;
    }

    void lockCamera(object sender, EventArgs e){
        locked = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!locked){

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

            //Adjust zoom
            targetZoom += (-scroll) * scrollSpeed * Time.deltaTime / (float) Math.Exp(-Time.deltaTime);
            targetZoom = Mathf.Clamp(targetZoom, 8f, 16f);
            float diff = targetZoom - Camera.main.orthographicSize;
            Camera.main.orthographicSize += diff / (1f + smoothingFactor * (float) Math.Exp(-Time.deltaTime));

            //Move the camera up/down/left/right
            float zoomLevel = (16.0f - Camera.main.orthographicSize) / 8.0f; // 0f = fully zoomed out, 1f = fully zoomed in
            newPos.x = Mathf.Clamp(newPos.x, - 14.0f * zoomLevel, 14.0f * zoomLevel);
            newPos.y = Mathf.Clamp(newPos.y, - 6.0f * zoomLevel - 8.0f , 7.5f * zoomLevel + 4.5f);

            transform.position = newPos;
        }
    }
}
