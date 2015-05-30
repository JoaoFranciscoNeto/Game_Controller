using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server_Mc_Advertiser : MonoBehaviour {


    UdpClient udpclient;
    IPEndPoint remoteep;
    Byte[] buffer;

	// Use this for initialization
	void Start () {
         udpclient = new UdpClient();

        IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
        udpclient.JoinMulticastGroup(multicastaddress);
        remoteep = new IPEndPoint(multicastaddress, 2223);

       buffer = null;

        //Console.WriteLine("Press ENTER to start sending messages");
        //Console.ReadLine();

        /*
        for (int i = 0; i <= 8000; i++)
        {
            buffer = Encoding.Unicode.GetBytes(i.ToString());
            udpclient.Send(buffer, buffer.Length, remoteep);
            Debug.Log("Sent " + i);
        }
         */

        //Console.WriteLine("All Done! Press ENTER to quit.");
        //Console.ReadLine();
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {



        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("jump");
            buffer = Encoding.Unicode.GetBytes("oi gatos");
            udpclient.Send(buffer, buffer.Length, remoteep);
            Debug.Log("Sent " + "gatos");
        }


       

	
	}
}
