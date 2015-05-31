using UnityEngine;
using System.Collections;
using System.Net;
using System;


public class Client_Join_Lobby : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
       
	}

    public static Boolean joinLobby(string serverIp)
    {
        int joinTries = 0;
        int timeout = 500;

        while (joinTries < 3)
        {
            NetworkHandler.sendMessage("JOIN", IPAddress.Parse(serverIp), 2224);

            Message msg = NetworkHandler.receiveMessage(2224, IPAddress.Parse(serverIp), timeout);

            Debug.Log(msg.body);

            if (msg.body != null && msg.body.Equals("OK"))
            {
                return true;
            }
            else
            {
                joinTries++;
                Debug.Log("Receive Timed Out on try number " + joinTries);
                timeout *= 2;
            }
        }

        return false;

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
