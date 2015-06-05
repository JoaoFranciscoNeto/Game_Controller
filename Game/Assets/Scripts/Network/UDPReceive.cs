
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public
    // public string IP = "127.0.0.1"; default local
    public int ReceivePort; // define > init
    
    // start from unity3d
    public void Start()
    {

        init();
    }

    // init
    private void init()
    {



        receiveThread = new Thread(
            new ThreadStart(ReceiveData));

        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(ReceivePort);
        while (true)
        {

            try
            {

                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, ReceivePort);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);


                Play_Object json = JsonConvert.DeserializeObject<Play_Object>(text);


                Debug.Log(text);


            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    } 

}