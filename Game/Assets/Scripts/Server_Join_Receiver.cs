using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class Server_Join_Receiver : MonoBehaviour {

	// Use this for initialization
     int listenerPort;
    IPEndPoint remote_end, client_end;
    
    UdpClient udpClient;
    

	void Start () {
        StartJoinReceiver();
	}

    void StartJoinReceiver()
    {
        listenerPort = 2224;

        remote_end = new IPEndPoint(IPAddress.Any, listenerPort);
        udpClient = new UdpClient(remote_end);
        udpClient.BeginReceive(new AsyncCallback(JoinLookup), null);

    }

    void JoinLookup(IAsyncResult result)
    {
        //received join message
        byte[] receiveBytes = udpClient.EndReceive(result, ref remote_end);

        //sending confirmation message to client

        IPAddress clientAddress = IPAddress.Parse(remote_end.Address.ToString());
        
        Debug.Log("received join from " + clientAddress.ToString());
        client_end = new IPEndPoint(clientAddress, listenerPort);

        Byte[] buffer = Encoding.Unicode.GetBytes("OK");
        udpClient.Send(buffer, buffer.Length, client_end);
        Debug.Log("OK sent to client");

       
        string strData = System.Text.Encoding.Unicode.GetString(receiveBytes);
        
        Debug.Log(strData);
        udpClient.BeginReceive(new AsyncCallback(JoinLookup), null);

    }
}
