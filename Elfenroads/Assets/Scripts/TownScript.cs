using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class TownScript : MonoBehaviour {
    [HideInInspector]
    public Town town { private set; get; }
    public string townName;
    public bool isStartingTown;

    // Awake gets called on all gameObjects before any Start(), so we use it to initialize towns which the roads will need.
    void Awake() {
        town = new Town(townName);
    }
}