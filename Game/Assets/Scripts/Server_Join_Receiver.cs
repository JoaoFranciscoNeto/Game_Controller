using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class Server_Join_Receiver : MonoBehaviour
{

    // Use this for initialization
    public int listenerPort = 3001;
    public int sendPort = 3002;
    IPEndPoint remote_end, client_end;

    private string pattern = @"^join;(\w+);(\w+(\s+\w+)*)$";

    UdpClient udpClient;

    Boolean listening = true;
    void Start()
    {
        StartJoinReceiver();
    }

    void OnDestroy()
    {
        listening = false;
    }

    void StartJoinReceiver()
    {
        listenerPort = 3000;

        remote_end = new IPEndPoint(IPAddress.Any, listenerPort);
        udpClient = new UdpClient(remote_end);
        udpClient.BeginReceive(new AsyncCallback(JoinLookup), null);

    }

    void JoinLookup(IAsyncResult result)
    {
        //received join message
        byte[] receiveBytes = udpClient.EndReceive(result, ref remote_end);

        //sending confirmation message to client

        string strData = System.Text.Encoding.Unicode.GetString(receiveBytes);

        Debug.Log("Received " + strData);

        if (Regex.IsMatch(strData,pattern))
        {
            Match m = Regex.Match(strData, pattern);

            IPAddress clientAddress = IPAddress.Parse(remote_end.Address.ToString());

            Debug.Log("received join from " + clientAddress.ToString());

            Player newPlayer = new Player();
            newPlayer.uniqueID = m.Groups[1].Value;
            newPlayer.userName = m.Groups[2].Value;
            ApplicationModel.controllers[clientAddress] = newPlayer;

            Debug.Log("Added new player " + newPlayer.ToString());
            Debug.Log(ApplicationModel.controllers.Count);
            NetworkHandler.sendMessage("ok", clientAddress, sendPort);

        }



        if (listening)
            udpClient.BeginReceive(new AsyncCallback(JoinLookup), null);

    }
}
