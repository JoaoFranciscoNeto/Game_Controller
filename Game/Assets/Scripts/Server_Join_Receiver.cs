using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class Server_Join_Receiver : MonoBehaviour
{

    // Use this for initialization
    public int listenerPort = 3001;
    public int sendPort = 3002;
    IPEndPoint remote_end, client_end;

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

        if (strData == "join")
        {
            IPAddress clientAddress = IPAddress.Parse(remote_end.Address.ToString());
            NetworkHandler.sendMessage("ok", clientAddress, sendPort);

            Debug.Log("received join from " + clientAddress.ToString());
            
            /*
            Debug.Log("received join from " + clientAddress.ToString());
            client_end = new IPEndPoint(clientAddress, 3000);

            Byte[] buffer = Encoding.Unicode.GetBytes("OK");
            udpClient.Send(buffer, buffer.Length, client_end);
            Debug.Log("OK sent to client");
             * */
        }



        if (listening)
            udpClient.BeginReceive(new AsyncCallback(JoinLookup), null);

    }
}
