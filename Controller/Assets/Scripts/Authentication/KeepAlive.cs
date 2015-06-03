using UnityEngine;
using System.Collections;
using System.Net;
using System.Threading;

public class KeepAlive {

    public static int checkWindow = 1000;
    public static int replication = 10;

    public static int pingsPort = 3003;

    public static void sendPings(IPAddress addr)
    {
        int sleepTime = checkWindow / replication;

        while (true)
        {
            NetworkHandler.sendMessage("ping;" + ApplicationModel.identifier, addr, pingsPort);
            //Debug.Log("Sent ping;"+ApplicationModel.identifier);
            Thread.Sleep(sleepTime);
        }
        
    }
}
