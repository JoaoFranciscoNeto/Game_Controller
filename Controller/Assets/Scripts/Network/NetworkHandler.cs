using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class NetworkHandler
{


    public static void sendMessage(string message, IPAddress address, int port)
    {
        UdpClient client = new UdpClient();
        IPEndPoint clientEnd = new IPEndPoint(address, port);
        Byte[] buffer = Encoding.Unicode.GetBytes(message);
        client.Send(buffer, buffer.Length, clientEnd);

        client.Close();
    }

    public static Message receiveMessage(int port)
    {
        return receiveMessage(port, IPAddress.Any);
    }

    public static Message receiveMessage(int port, IPAddress address)
    {
        return receiveMessage(port, address, 0);
    }


    public static Message receiveMessage(int port, IPAddress address, int timeout)
    {
        UdpClient client = new UdpClient(port);
        IPEndPoint remoteEnd = new IPEndPoint(address, 0);
        
        Byte[] receiveBytes = client.Receive(ref remoteEnd);
        if(timeout>0)
        {
            client.Client.ReceiveTimeout = timeout;
        }
        Message result = new Message();
        result.ip = remoteEnd.Address;
        result.port = remoteEnd.Port;
        result.body = Encoding.Unicode.GetString(receiveBytes);
        

        client.Close();


        return result;
    }
  
}

public struct Message
{
    public IPAddress ip;
    public int port;
    public string body;
}