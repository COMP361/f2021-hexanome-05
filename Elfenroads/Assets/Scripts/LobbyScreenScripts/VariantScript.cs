// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unityengine.UI;

// public class VariantScript : MonoBehaviour
// {
//     private Session mySession = null;
//     private Client client = client.Instance();

//     private Dropdown dropdown;

//     void Start(){
//         modeDropDown.onClick.AddListener(selectVariant);
//     }

//     public void setSession(Session aSession) {
//         mySession = aSession;
//     }
    
//     public async void selectVariant(){
//         if(mySession == null){
//             Debug.Log("Error in variant dropdown, session was never set");
//         } else{
//             await client.refreshSessions();
//         }
//     }
// }
