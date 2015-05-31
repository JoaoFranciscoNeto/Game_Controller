using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEngine.UI;



public class ServerFinding : MonoBehaviour
{

    public int startupPort = 2223;
    public string groupAddress = "224.0.0.3";

    public string pattern = @"GameController(\w+)";

    string remoteAddress;

    public Boolean searchingService = true;

    UdpClient udpClient;
    IPEndPoint remote_end;


    Dictionary<string, string> serverAddresses = new Dictionary<string, string>();

    Text log;

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
        //log.text = "Begin Receive";

        udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
    }

    void ServerLookup(IAsyncResult result)
    {
        remote_end = new IPEndPoint(IPAddress.Any, startupPort);
        byte[] receiveBytes = udpClient.EndReceive(result, ref remote_end);


        string strData = System.Text.Encoding.Unicode.GetString(receiveBytes);
        Debug.Log(strData);

        if (strData.Equals("GameController"))
        {
            remoteAddress = remote_end.Address.ToString();

            if (Client_Join_Lobby.joinLobby(remoteAddress))
            {
                return;
            }

        }
        else
        {
            Debug.Log("mensagem invalida");

        }

        Debug.Log("Received " + strData + " from " + remote_end.Address.ToString());
        Debug.Log("servers size:" + serverAddresses.Count);
        if (searchingService)
        {
            udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
        }
    }


}
