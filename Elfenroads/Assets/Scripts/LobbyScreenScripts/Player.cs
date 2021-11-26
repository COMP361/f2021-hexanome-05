using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string name;
    private string password;
    private string authToken;

    public Player(){

    }

    public string getName(){
        return name;
    }

    public string getPassword(){
        return password;
    }

    public string getToken(){
        return authToken;
    }

    public void setName(string s){
        name = s;
    }

    public void setPassword(string s){
        password = s;
    }

    public void setToken(string s){
        authToken = s;
    }

}
