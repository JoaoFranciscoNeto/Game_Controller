using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading;



public class FindServer : MonoBehaviour
{

    public int startupPort = 2223;
    public string groupAddress = "224.0.0.3";
    public int joinSendPort = 3000;
    public int joinReceivePort = 3002;

    public int retryAttempts = 3;


    public Boolean searchingService = true;

    UdpClient udpClient;
    IPEndPoint remote_end;
    private string pattern = @"^GameController;(\w+)$";

    Dictionary<string, string> serverAddresses = new Dictionary<string, string>();

    List<GameObject> buttons = new List<GameObject>();

    public GameObject sampleButton;

    public Transform parentContainer;

    private Text userName;

    void Start()
    {
        userName = GameObject.FindGameObjectWithTag("UserName").GetComponent<Text>();
        FindServers();
    }

    void OnDestroy()
    {
        searchingService = false;
    }


    public void FindServers()
    {


        remote_end = new IPEndPoint(IPAddress.Any, startupPort);
        udpClient = new UdpClient(remote_end);
        udpClient.JoinMulticastGroup(IPAddress.Parse(groupAddress));

        searchingService = true;
        Thread listener = new Thread(new ThreadStart(StartGameClient));
        listener.Start();

        Thread.Sleep(2000);
        searchingService = false;

        listener.Abort();
        listener.Join();

        udpClient.Close();

        foreach (GameObject btn in buttons)
        {
            Destroy(btn);
        }

        foreach (KeyValuePair<string, string> entry in serverAddresses)
        {
            addButton(entry.Value, entry.Key);
        }

    }

    void StartGameClient()
    {
        udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
    }

    void ServerLookup(IAsyncResult result)
    {
        remote_end = new IPEndPoint(IPAddress.Any, startupPort);
        byte[] receiveBytes = udpClient.EndReceive(result, ref remote_end);
        string strData = System.Text.Encoding.Unicode.GetString(receiveBytes);

        if (Regex.IsMatch(strData, pattern))
        {

            Match m = Regex.Match(strData, pattern);

            serverAddresses[remote_end.Address.ToString()] = m.Groups[1].Value;


        }
        else
        {
            Debug.LogWarning("Another application is running on Multicast Address: " + groupAddress);
        }

        if (searchingService)
        {
            udpClient.BeginReceive(new AsyncCallback(ServerLookup), null);
        }
    }


    private void addButton(string sName, string ip)
    {
        GameObject newButton = Instantiate(sampleButton) as GameObject;
        SampleButton button = newButton.GetComponent<SampleButton>();

        button.nameLabel.text = sName;
        button.ipLabel.text = ip;
        newButton.transform.SetParent(parentContainer, false);

        Button b = button.GetComponent<Button>();
        b.onClick.AddListener(() => joinLobby(ip));

        buttons.Add(newButton);
    }

    public void joinLobby(string serverIp)
    {
        Debug.Log("Joining Server " + serverIp);

        int joinTries = 0;
        int timeout = 500;

        string uName;

        if (userName.text == "" || !Regex.IsMatch(userName.text,@"(\w+(\s+\w+)*)",RegexOptions.IgnoreCase))
            uName = "Douchebag";
        else
            uName = userName.text;

        string message = "join;" + SystemInfo.deviceUniqueIdentifier + ";" + uName;

        while (joinTries < retryAttempts)
        {
            NetworkHandler.sendMessage(message, IPAddress.Parse(serverIp), joinSendPort);

            Message msg = NetworkHandler.receiveMessage(joinReceivePort, IPAddress.Parse(serverIp), timeout);

            Debug.Log("Message received = " + msg.body);

            if (msg.body != null && msg.body.Equals("ok"))
            {

                return;
            }
            else
            {
                joinTries++;
                Debug.Log("Receive Timed Out on try number " + joinTries);
                timeout *= 2;
            }
        }
    }
}
