using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string name;
    private string password;
    private string accessToken;
    private string refreshToken;

    public Player(){

    }

    public string getName(){
        return name;
    }

    public string getPassword(){
        return password;
    }

    public string getAccToken(){
        return accessToken;
    }

    public string getRefToken(){
        return refreshToken;
    }

    public void setName(string s){
        name = s;
    }

    public void setPassword(string s){
        password = s;
    }

    public void setAccToken(string s){
        accessToken = s;
    }

    public void setRefToken(string s){
        refreshToken = s;
    }
}
