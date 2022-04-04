using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controls;

public class QuitChecker : MonoBehaviour
{
    void OnApplicationQuit(){
        Elfenroads.Control.requestQuit();
    }
}
