using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

public class ClickManager : MonoBehaviour
{

    //May want to have "lockClick" functionality here. Alternative would be that each manager has their own "locked" attribute (see MoveBootController)

    // Update is called once per frame
    void Update()
    {
        if(!(Elfenroads.Model == null || Elfenroads.Model.game == null || Elfenroads.Model.game.currentPhase  == null)){
            if(Input.GetMouseButtonDown(0)){
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if(hit.collider != null){
                    //Later, may want to structure so that ALL clickable game objects have an "onClick" function / inherit from the same class which has such a function.
                    if(hit.collider.gameObject.GetComponent<RoadView>() != null){
                        //We clicked on a road
                        if((! EventSystem.current.IsPointerOverGameObject()) && Elfenroads.Model.game.currentPhase is MoveBoot){
                            hit.collider.gameObject.GetComponent<RoadView>().OnClick();
                        } 
                    }
                }
            }
        }
    }
}
