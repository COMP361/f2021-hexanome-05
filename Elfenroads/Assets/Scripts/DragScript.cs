using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform draggableElement;
    public RectTransform canvas;
    public string draggableType;

    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 startingPos;
    private bool locked = true;

    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f){
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;
        while (elapsedTime < duration){
            r.localPosition = Vector2.Lerp(startingPos,targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        r.localPosition = targetPosition;
    }

    public IEnumerator getPositions(){
        yield return new WaitForEndOfFrame();
        startingPos = draggableElement.localPosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("getPositions");
        Elfenroads.Control.LockDraggables += lockDrag;
        Elfenroads.Control.UnlockDraggables += unlockDrag;
    }

    public void resetStartingPositions(){
        StartCoroutine("getPositions");
    }


    public void OnBeginDrag(PointerEventData data){
        if(!locked){
            mOriginalPanelLocalPosition = draggableElement.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
        }
    }

    public void OnDrag(PointerEventData data){
        if(!locked){
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, data.position, data.pressEventCamera, out localPointerPosition)){
                Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
                draggableElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if(!locked){
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hitInfo.collider != null)
            {
                RoadView rv = hitInfo.collider.GetComponent<RoadView>();
                if(rv != null){
                    if(gameObject.tag == "Card"){
                        rv.cardDragged(draggableType);
                    }else if(gameObject.tag == "Counter"){
                        rv.counterDragged(draggableType);
                    }
                }
            }
            StartCoroutine(Coroutine_MoveUIElement(draggableElement, startingPos, 0.5f)); 
        }
    }
       
    private void lockDrag(object sender, EventArgs e){
        locked = true;
    }

    private void unlockDrag(object sender, EventArgs e){
        locked = false;
    }
}
