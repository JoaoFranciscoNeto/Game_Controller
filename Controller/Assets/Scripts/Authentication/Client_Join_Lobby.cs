using UnityEngine;
using System.Collections;
using System.Net;
using System;


public class Client_Join_Lobby : MonoBehaviour {
    
    public static int timeout = 1000;

	// Use this for initialization
	void Start () {
       
	}

    public static Boolean joinLobby(string serverIp)
    {
        int joinTries = 0;
        while (joinTries < 3)
        {
            NetworkHandler.sendMessage("JOIN", IPAddress.Parse(serverIp), 2224);

            if (NetworkHandler.receiveMessage(2224, IPAddress.Parse(serverIp), timeout).body.Equals("OK"))
            {
                return true;
            }
            else
            {
                timeout *= 2;
                joinTries++;
            }
        }

        return false;

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
