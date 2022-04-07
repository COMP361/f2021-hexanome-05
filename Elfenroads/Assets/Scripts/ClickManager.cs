using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

public class ClickManager : MonoBehaviour
{
    public LayerMask counters;
    //May want to have "lockClick" functionality here. Alternative would be that each manager has their own "locked" attribute (see MoveBootController)

    // Update is called once per frame
    void Update()
    {
        if(! (Elfenroads.Model == null || Elfenroads.Model.game == null || Elfenroads.Model.game.currentPhase  == null) ){
            if(Input.GetMouseButtonDown(0)){
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, 100);

                for(int i = 0; i < hits.Length ; i++){
                    RaycastHit2D curHit = hits[i];

                    if(curHit.collider != null){
                    //Later, may want to structure so that ALL clickable game objects have an "onClick" function / inherit from the same class which has such a function.
                        if(curHit.collider.gameObject.GetComponent<RoadView>() != null){
                            //We clicked on a road
                            if((! EventSystem.current.IsPointerOverGameObject()) && Elfenroads.Model.game.currentPhase is MoveBoot){
                                Debug.Log("Road clicked!");
                                curHit.collider.gameObject.GetComponent<RoadView>().OnClick();
                            } 
                        }
                        if(curHit.collider.gameObject.GetComponent<TownView>() != null){
                            if((! EventSystem.current.IsPointerOverGameObject()) && Elfenroads.Model.game.currentPhase is MoveBoot && Elfenroads.Model.game.variant.HasFlag(Game.Variant.ElfenWitch)){
                                Debug.Log("Town clicked!");
                                curHit.collider.gameObject.GetComponent<TownView>().OnClick();
                            } 
                        }
                        if(curHit.collider.gameObject.GetComponent<CounterClickerScript>() != null){
                            if((! EventSystem.current.IsPointerOverGameObject()) && Elfenroads.Model.game.currentPhase is PlanTravelRoutes && Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                                    Debug.Log("Counter clicked!");
                                    curHit.collider.gameObject.GetComponent<CounterClickerScript>().OnClick();
                                }
                        } 
                    }
                }
            }
        }
    }
}
