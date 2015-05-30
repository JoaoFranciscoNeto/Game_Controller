using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server_Mc_Advertiser : MonoBehaviour
{

    public string mcAddress = "239.0.0.222";
    public int mcPort = 2223;

    UdpClient udpclient;
    IPEndPoint remoteep;
    Byte[] buffer;

    // Use this for initialization
    void Start()
    {
        udpclient = new UdpClient();

        IPAddress multicastaddress = IPAddress.Parse(mcAddress);
        udpclient.JoinMulticastGroup(multicastaddress);
        remoteep = new IPEndPoint(multicastaddress, mcPort);

        buffer = null;

        StartCoroutine(BroadCast("GameController"));

    }

    IEnumerator BroadCast(string message)
    {
        buffer = Encoding.Unicode.GetBytes(message);
        while (true)
        {
            udpclient.Send(buffer, buffer.Length, remoteep);
            Debug.Log("Broadcasted Service");
            yield return new WaitForSeconds(1);
        }
     
    }
        
}
