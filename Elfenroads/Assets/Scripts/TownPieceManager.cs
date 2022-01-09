using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPieceManager : MonoBehaviour
{
    public Sprite blueSprite;
    public Sprite YellowSprite;
    public GameObject town;
    private BootColor aColor;

    private GameObject owner;

    public void setColor(BootColor pColor){
        aColor = pColor;
    }

    public BootColor getColor(){
        return aColor;
    }

}
