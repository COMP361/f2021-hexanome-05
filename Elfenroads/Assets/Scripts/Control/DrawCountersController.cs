using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCountersController : MonoBehaviour
{
    public PlayerInfoController playerInfoController;
    public InfoWindowController infoWindowController;
    //Here's the general "flow" of all phases. An object (say, a counter or a road) gets clicked, and in some "onClick()" function, it will check to see if the logged-in client is the Game's currentPlayer, and that it is the correct phase.
    //If not, nothing happens. If so, it will call (or raise an event, whatever works but I think calls are easier) its associated PhaseController (eg. DrawCountersController). The Controller will then validate the move, and either respond with
    //some kind of "invalid" feedback or it will call the main "ElfenroadsController" to send a command to the Server.

    public void validateDrawCounter(GameObject clickedCounter){ //Parameter to be decided. CurrentPlayer can be found through Elfenroads.Model, so either an integer representing the index of the counter chosen, or it will simply pass in the Counter / that counter's Guid.
        if(playerInfoController.windowOpen || infoWindowController.isOpen){
            return;
        }
        //Is validation even needed here? If we make it to this point, a counter was clicked and there's no "wrong" counter to click. For now, I'll work as if there is nothing to validate here (because I don't think there is )
        Elfenroads.Control.drawCounter(clickedCounter);

    }


    public void validateDrawRandomCounter(){ //No parameter needed, since we'll just tell the Server to draw a random counter from the counterpile.
        if(playerInfoController.windowOpen || infoWindowController.isOpen){
            return;
        }
        //Is validation even needed here? If we make it to this point, a counter was clicked and there's no "wrong" choice. For now, I'll work as if there is nothing to validate.
        Elfenroads.Control.drawRandomCounter();

    }

}
