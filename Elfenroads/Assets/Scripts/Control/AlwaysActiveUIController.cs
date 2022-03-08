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

        // switch to display chooseBoot help info
        //////////////////////////////////////////// To be edited later ////////////////////////
        PhaseName.text = "This is choose boot";
        PhaseExplanation.text = "A very long string, explanation for choose boot aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        //////////////////////////////////////////// To be edited later ////////////////////////
	}

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
