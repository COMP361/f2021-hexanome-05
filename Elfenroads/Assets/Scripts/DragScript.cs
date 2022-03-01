using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform draggableElement;
    public RectTransform canvas;

    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 startingPos;

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
    }

    public void OnBeginDrag(PointerEventData data){
        mOriginalPanelLocalPosition = draggableElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data){
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, data.position, data.pressEventCamera, out localPointerPosition)){
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
            draggableElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    public void OnEndDrag(PointerEventData eventData){

        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
 
        if(hitInfo.collider != null)
        {
            if(hitInfo.collider.GetComponent<RoadView>() != null){
                Debug.Log("Collided with a road!");
            }
        }

        StartCoroutine(Coroutine_MoveUIElement(draggableElement, startingPos, 0.5f)); 
           
    }
       
        // RaycastHit hit;
        // Ray ray = Camera.main.ScreenPointToRay(
        // Input.mousePosition);
    
        // if (Physics.Raycast(ray, out hit, 1000.0f)){
        //     //Check collision against road, call function according to tag.
        //     if(hit.collider.GetComponent<RoadView>() != null){
        //         Debug.Log("Dragged onto a road!");
        //     }
            
        // }else{
        //     StartCoroutine(Coroutine_MoveUIElement(draggableElement, startingPos, 0.5f));
        // }
}
