using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Models
{
    public class Player
    {
        private string name;
        private string password;
        private string accessToken;
        private string refreshToken;


        private GameObject currentCity;
        public Vector3 Offset;
        public Sprite blueSprite;
        private BootColor aColor;
        private GameObject aInventory;
        private string playerName;

        public Player()
        {

        }

        public void setInventory(GameObject inv)
        {
            aInventory = inv;
        }

        public GameObject getInventory()
        {
            return aInventory;
        }

        public string getname()
        {
            return playerName;
        }

        public string getName()
        {
            return name;
        }

        public string getPassword()
        {
            return password;
        }

        public string getAccToken()
        {
            return accessToken;
        }

        public string getRefToken()
        {
            return refreshToken;
        }

        public void setName(string name)
        {
            playerName = name;
        }

        public void setPassword(string s)
        {
            password = s;
        }

        public void setAccToken(string s)
        {
            accessToken = s;
        }

        public void setRefToken(string s)
        {
            refreshToken = s;
        }
    }
}
