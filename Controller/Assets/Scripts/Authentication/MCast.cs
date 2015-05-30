using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;



public class MCast : MonoBehaviour
{

    public int startupPort = 2223;
    public string groupAddress = "224.0.0.3";

    string remoteAddress;
    public int port = 2224;

    public Boolean searchingService = true;

    UdpClient udpClient;
    IPEndPoint remote_end;


    HashSet<String> serverAddresses = new HashSet<String>();
    

    // Use this for initialization
    void Start()
    {
        StartGameClient();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        searchingService = false;
    }

    void StartGameClient()
    {

        remote_end = new IPEndPoint(IPAddress.Any, startupPort);
        udpClient = new UdpClient(remote_end);
        udpClient.JoinMulticastGroup(IPAddress.Parse(groupAddress));

        Debug.Log("Begin Receive");

        udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
    }

    void ServerLookup(IAsyncResult result)
    {
        byte[] receiveBytes = udpClient.EndReceive(result, ref remote_end);
        string strData = System.Text.Encoding.Unicode.GetString(receiveBytes);



        if(strData.Equals("GameController"))
        {
        remoteAddress = remote_end.Address.ToString();
        serverAddresses.Add(remoteAddress);
        Client_Join_Lobby.joinLobby(remoteAddress);
 
        }
        else
        {
            Debug.Log("mensagem invalida");
            
        }
        
        Debug.Log("Received " + strData + " from " + remote_end.Address.ToString());
        Debug.Log("servers size:" + serverAddresses.Count);
        if(searchingService)
        {
            udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
        }
    }


}
