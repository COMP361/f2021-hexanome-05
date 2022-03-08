using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class InfoWindowController : MonoBehaviour
{

    public GameObject helpWindow;
    public TMPro.TMP_Text PhaseName;
    public TMPro.TMP_Text PhaseExplanation;
    public PlayerInfoController playerInfoController;

    [HideInInspector]
    public bool isOpen = false;
    
    private bool cameraOnOpen = true;
    private bool draggablesOnOpen = true;


	void Start()
	{
        helpWindow.SetActive(false);

        PhaseName.text = "Choose Your Boot";
        PhaseExplanation.text = "Please click on any of the select buttons to choose your boot." + "\n" +
        "A grey button indicates that another player has already chosen this boot.";
	}

    public void UpdateDrawCounterHelp(){
        PhaseName.text = "Draw Counters";
        PhaseExplanation.text = "The goal of this phase is to add counters to your inventory for use in the PlanTravel phase." + "\n" +
                                "- Please click on the desired counter to keep it. You could also click the counter pile to draw randomly." + "\n" +
                                "- Your inventory is shown at the bottom of the screen. Click 'expand' to see your cards!" + "\n" +
                                "- The map button in the top right corner will toggle the map on and off." + "\n" +
                                "- Once in the map view, you can use WASD to pan around the map, or zoom in/out with the mouse wheel.";
    }

    public void UpdatePlanTravelRoutesHelp(){
        PhaseName.text = "Plan Travel Routes";
        PhaseExplanation.text = "The goal of this phase is to place counters, enabling you to use cards on them in the next phase." + "\n" + 
                                "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" + 
                                "- To place a counter/obstacle on the road, drag the item from your inventory to the road." + "\n" +
                                "- If you do not have anymore counters to place, click pass.";
    }

    
    public void UpdateMoveBootHelp(){
        PhaseName.text = "Move Your ElfenBoot";
        PhaseExplanation.text = "The goal of this phase is to play cards on roads with counters to visit Towns and earn points!" + "\n" +
                                "You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" +
                                "To move your boot, drag the card type you want to use on the target road, lake or stream." + "\n" +
                                "You can take many moves until you are satisfied, at which point you can click 'EndTurn'." + "\n" +
                                "However, if you have more than 4 cards when you try to end, you must discard any extras.";
    }

    
    public void UpdateFinishRoundHelp(){
        PhaseName.text = "Finish The Round";
        PhaseExplanation.text = "I will be adding help text here once we implemented this phase";
    }


    /*

    void Update(){
        if (Elfenroads.Model.game.currentPhase is DrawCounters){
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is draw counters phase";
            PhaseExplanation.text = "A very long string, explanation for draw counters aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////

        }
        else if (Elfenroads.Model.game.currentPhase is Auction){
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is auction phase";
            PhaseExplanation.text = "A very long string, explanation for auction aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////
        }
        else if (Elfenroads.Model.game.currentPhase is FinishRound){
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is finish round phase";
            PhaseExplanation.text = "A very long string, explanation for finish round aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////
        }
        else if (Elfenroads.Model.game.currentPhase is PlanTravelRoutes){
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is plan travel route phase";
            PhaseExplanation.text = "A very long string, explanation for plan travel route aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////
        }
        else if (Elfenroads.Model.game.currentPhase is MoveBoot){
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is move boot phase";
            PhaseExplanation.text = "A very long string, explanation for move boot aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////
        }
        ////////////////////////// more phases to be added once they are defined //////////////////
        else {
            //////////////////////////////////////////// To be edited later ////////////////////////
            PhaseName.text = "This is choose boot";
            PhaseExplanation.text = "A very long string, explanation for choose boot aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //////////////////////////////////////////// To be edited later ////////////////////////
        }
    }

    */

    public void CloseHelpWindow(){
        if(helpWindow.activeSelf){
            isOpen = false;
            if(cameraOnOpen){
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
            }else{
                Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            }
            if(draggablesOnOpen){
                Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
            }else{
                 Elfenroads.Control.UnlockDraggables?.Invoke(null, EventArgs.Empty);
            }
            helpWindow.SetActive(false);
        }
    }

    public void ShowHelpWindow(){
        if(playerInfoController.windowOpen){
            return;
        }
        if(!helpWindow.activeSelf){
            isOpen = true;
            cameraOnOpen = Elfenroads.Control.cameraLocked;
            draggablesOnOpen = Elfenroads.Control.draggablesLocked;
            Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
            Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
            helpWindow.SetActive(true);
        }
    }
    
}
