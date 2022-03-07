using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class PlayerInfoController : MonoBehaviour
{
    //This controller serves to show player inventory information.
    public GameObject opponentTabPrefab;
    public GameObject inventoryWindow;
    public RectTransform opponentsLayout;
    public RectTransform playerCounters;
    public RectTransform playerCards;
    public TMPro.TMP_Text playerName;
    public TMPro.TMP_Text playerStats;

    [Header("CounterPrefabs")]
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject dragonCounterPrefab;
    public GameObject landCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject backOfCounterPrefab;
    [Header("CardPrefabs")]
    public GameObject cloudCardPrefab;
    public GameObject cycleCardPrefab;
    public GameObject dragonCardPrefab;
    public GameObject pigCardPrefabPrefab;
    public GameObject raftCardPrefab;
    public GameObject trollCardPrefab;
    public GameObject unicornCardPrefab;
    public GameObject backOfCardPrefab;


    //Called on initial game setup.
    public void createOpponentPrefabs(List<Player> players){
        foreach(Player p in players){
            if(p.name == GameObject.Find("SessionInfo").GetComponent<SessionInfo>().getClient().clientCredentials.username){
                continue;
            }
            GameObject instantiatedTab = Instantiate(opponentTabPrefab, opponentsLayout);
            instantiatedTab.GetComponent<OpponentPlayerView>().setAndSubscribeToModel(p);

            switch(p.boot.color){
                case Models.Color.RED:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(255, 76, 76, 221);
                    instantiatedTab.GetComponent<Image>().color = new Color32(221, 76, 76, 255);
                    break;
                }
                case Models.Color.BLUE:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(50, 127, 210, 255);
                    break;
                }
                case Models.Color.GREEN:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(37, 131, 59, 255);
                    break;
                }
                case Models.Color.PURPLE:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(169, 51, 203, 255);
                    break;
                }
                case Models.Color.YELLOW:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(253, 223, 20, 255);
                    break;
                }
                case Models.Color.BLACK:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(42, 42, 44, 255);
                    instantiatedTab.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                    instantiatedTab.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                    break;
                }
                default: Debug.Log("Invalid color!"); break;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(opponentsLayout);
    }

    public void openAndSetupWindow(Player targetPlayer){

    }
}
