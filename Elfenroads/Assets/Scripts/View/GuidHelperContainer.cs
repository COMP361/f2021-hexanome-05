using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;
using Models;
using System;


namespace Views {
    public interface GuidHelperContainer{

        public abstract void GUIClicked(GameObject clickedObject);
    }
}