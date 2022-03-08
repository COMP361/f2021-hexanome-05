using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class AlwaysActiveUIController : MonoBehaviour
{

    public GameObject helpWindow;
    public TMPro.TMP_Text PhaseName;
    public TMPro.TMP_Text PhaseExplanation;


	void Start()
	{
        helpWindow.SetActive(false);

        PhaseName.text = "Choose Your Boot";
        PhaseExplanation.text = "Please click on any of the select buttons to choose your boot." + "\n" +
        "A grey button indicates that another player has already chosen this boot.";
	}

    public void UpdateDrawCounterHelp(){
        PhaseName.text = "Draw Counters";
        PhaseExplanation.text = "Please click on the desired counter to keep it. You could also draw from the face-down counter pile." + "\n" +
                                "Your inventory is shown at the bottom of the screen." + "\n" +
                                "To view the ElfenRoads game map before selecting a counter, click the map button on the top right corner." + "\n" +
                                "Once in the map view, you can press wasd to pan around the map, or zoom around with the mouse wheel." + "\n" +
                                "To bring back the counter selection window, click on the map button again.";
    }

    public void UpdatePlanTravelRoutesHelp(){
        PhaseName.text = "Plan Travel Route";
        PhaseExplanation.text = "You can press wasd to pan around the map, or zoom around with the mouse wheel." + "\n" + 
                                "To place a counter/obstacle on the road, drag the item from your inventory to the road." + "\n" +
                                "If you do not have anymore counters to place, click pass.";
    }

    
    public void UpdateMoveBootHelp(){
        PhaseName.text = "Move Your ElfenBoot";
        PhaseExplanation.text = "You can press wasd to pan around the map, or zoom with the mouse wheel." + "\n" +
                                "To move your boot, you can either click on the city that you wish to travel to, or the road/river that you wish to take." + "\n" +
                                "You can take many moves untill you are satisfied." + "\n" +
                                "After that, clicking on the EndTurn button will take your boot to the selected destination and bring you to select travel cards that you wish to keep.";
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
            helpWindow.SetActive(false);
            Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
        }
    }

    public void ShowHelpWindow(){
        if(!helpWindow.activeSelf){
                helpWindow.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
        }
    }
}
