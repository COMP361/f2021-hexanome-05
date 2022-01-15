using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;
using Models;


namespace Views
{
    public class BootView : MonoBehaviour
    {
        private Boot boot;

        public void setAndSubscribeToModel(Boot inputBoot){
            this.boot = inputBoot;
            boot.ModelUpdated += onModelUpdated;
        }

        void onModelUpdated(object sender, EventArgs e) {
            // Reflect changes here. In this case, must get the Town the boot in the model is on, and move the GameObject attached to this script to that Town GameObject.
            GameObject.Find("Townname");
        }
    }
}