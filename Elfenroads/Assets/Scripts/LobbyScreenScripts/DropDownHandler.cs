using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    public Dropdown dropdown;
    public bool hasJoined = false;
    async void OnEnable()
    {
        var savegames = await Client.Instance().getSavegames();
        dropdown.ClearOptions();
        dropdown.AddOptions(savegames);
        SessionInfo.Instance().savegame_id = "";
    }

    public void OnValueChanged()
    {
        SessionInfo.Instance().savegame_id = dropdown.options[dropdown.value].text;
    }

    void Update(){
        if(Client.Instance().mySession != null){
            dropdown.interactable = false;
        }else{
            dropdown.interactable = true;
        }
        if(!hasJoined){
            if(dropdown.value == 0){
            SessionInfo.Instance().savegame_id = "";
            }else{
                SessionInfo.Instance().savegame_id = dropdown.options[dropdown.value].text;
            }  
        }
    }
}
