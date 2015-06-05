


using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class UDPSend : MonoBehaviour
{
    private static int localPort;

    // prefs
    public string DestinationIP; 
    public int SendPort;  

   
    IPEndPoint remoteEndPoint;
    UdpClient client;

    
    // start from unity3d
    public void Start()
    {
        init();
    }


    // init
    public void init()
    {
        

        //redefinir se for preciso
        //IP = "127.0.0.1";
        //port = 8051;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(DestinationIP), SendPort);
        client = new UdpClient();

    }



    // sendData
    private void sendString(string message)
    {
        try
        {

            byte[] data = Encoding.UTF8.GetBytes(message);

            client.Send(data, data.Length, remoteEndPoint);
      
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    private void Update()
    {
        if (Input.GetButton("Jump"))
        {

            Play_Object play = new Play_Object("xyz", "123123");
            sendString(JsonConvert.SerializeObject(play));
            Debug.Log("sent " + JsonConvert.SerializeObject(play));
        }
    }

}