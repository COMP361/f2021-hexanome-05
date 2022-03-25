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
        PhaseName.text = "Round " + Elfenroads.Model.game.roundNumber +" - Draw Counters";
        PhaseExplanation.text = "The goal of this phase is to add counters to your inventory for use in the PlanTravel phase." + "\n" +
                                "- Please click on the desired counter to keep it. You could also click the counter pile to draw randomly." + "\n" +
                                "- Your inventory is shown at the bottom of the screen. Click 'expand' to see your cards!" + "\n" +
                                "- The map button in the top right corner will toggle the map on and off." + "\n" +
                                "- Once in the map view, you can use WASD to pan around the map, or zoom in/out with the mouse wheel.";
    }

    public void UpdatePlanTravelRoutesHelp(){
        PhaseName.text = "Round " + Elfenroads.Model.game.roundNumber + " - Plan Travel Routes";
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            PhaseExplanation.text = "The goal of this phase is to place counters, enabling you to use cards on them in the next phase." + "\n" + 
                                "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" + 
                                "- To place a counter/obstacle on the road, drag the item from your inventory to the road." + "\n" +
                                "- To use a Double spell, click the icon. Once enabled, it will allow you to drag counters onto already-occupied roads. Click the icon again to cancel." + "\n" +
                                "- To use an Exchange spell, click the icon, then click the two counters you would like to swap. Click the icon again to cancel." + "\n" +
                                "- If you do not have anymore counters to place, click pass.";
        }else{
            PhaseExplanation.text = "The goal of this phase is to place counters, enabling you to use cards on them in the next phase." + "\n" + 
                                "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" + 
                                "- To place a counter/obstacle on the road, drag the item from your inventory to the road." + "\n" +
                                "- If you do not have anymore counters to place, click pass.";
        }
    }

    
    public void UpdateMoveBootHelp(){
        PhaseName.text = "Round " + Elfenroads.Model.game.roundNumber + " - Move Your ElfenBoot";
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.ElfenWitch)){
            PhaseExplanation.text = "The goal of this phase is to play cards on roads with counters to visit Towns and earn points!" + "\n" +
                                    "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" +
                                    "- To move your boot, drag the card type you want to use on the target road, lake or stream. You will accumulate gold according to a town's gold value for each move." + "\n" +
                                    "- Alternatively, you can click a road to play a 'Caravan', where you play 3 or 4 of any card." + "\n" +
                                    "- You can move until you are satisfied, at which point you can click 'EndTurn'." + "\n" +
                                    "- Once ending, you can cash in your accumulated gold, or draw two TravelCards from the supply.";
        }else if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            PhaseExplanation.text = "The goal of this phase is to play cards on roads with counters to visit Towns and earn points!" + "\n" +
                                    "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" +
                                    "- To move your boot, drag the card type you want to use on the target road, lake or stream. You will accumulate gold according to a town's gold value for each move." + "\n" +
                                    "- Alternatively, you can click a road to play a 'Caravan', where you play 3 or 4 of any card." + "\n" +
                                    "- You can move until you are satisfied, at which point you can click 'EndTurn'." + "\n" +
                                    "- Once ending, you can cash in your accumulated gold, or draw two TravelCards from the supply." + "\n" +
                                    "- You can click the 'Witch' Button to activate her. While under its effect, you can click on a Town for a magic flight, or drag cards as usual, avoiding obstacle costs. Click the button again to cancel.";
        }else{
            PhaseExplanation.text = "The goal of this phase is to play cards on roads with counters to visit Towns and earn points!" + "\n" +
                                    "- You can use WASD to pan around the map, or zoom in/out with the mouse wheel." + "\n" +
                                    "- To move your boot, drag the card type you want to use on the target road, lake or stream." + "\n" +
                                    "- Alternatively, you can click a road to play a 'Caravan', where you play 3 or 4 of any card." + "\n" +
                                    "- You can move until you are satisfied, at which point you can click 'EndTurn'." + "\n" +
                                    "- However, if you have more than 4 cards when you try to end, you must discard any extras.";
        }
    }

    
    public void UpdateFinishRoundHelp(){
        PhaseName.text = "Round " + Elfenroads.Model.game.roundNumber + " - Finish The Round";
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            PhaseExplanation.text = "If you have more than 1 counter (not including the obstacle) you must discard any extras before moving on to the next round." + "\n" +
                                "-If you had one or less counters, just kick back and wait for other players to discard their extras!" + "\n" + 
                                "-You can still study the map using the Map button! Use WASD to pan around, and zoom in/out with the mouse wheel." + "\n" +
                                "-Checking your cards is a good idea to ensure you keep the best-possible counter!";
        }else{
            PhaseExplanation.text = "If you have more than 2 counters you must discard any extras before moving on to the next round." + "\n" +
                                "-If you had 2 or less counters, just kick back and wait for other players to discard their extras!" + "\n" + 
                                "-You can still study the map using the Map button! Use WASD to pan around, and zoom in/out with the mouse wheel." + "\n" +
                                "-Checking your cards and gold is a good idea to ensure you keep the most useful counters!";
        }
    }

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
