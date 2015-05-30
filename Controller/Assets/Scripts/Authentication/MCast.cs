using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;



public class MCast : MonoBehaviour
{

    public int startupPort = 5100;
    public string groupAddress = "239.0.0.222";

    public string remoteAddress = "192.168.2.117";
    public int port = 2224;

    UdpClient udpClient;
    IPEndPoint remote_end;

    NetworkHandler handler;

    // Use this for initialization
    void Start()
    {
        //StartGameClient();
    }

    void Update()
    {
        if (Input.GetButtonUp("Jump"))
        {
            NetworkHandler.sendMessage("join", IPAddress.Parse(remoteAddress), port);
        }
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
        Debug.Log("Received " + strData + " from " + remote_end.Address.ToString());

        udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);

    }


}
