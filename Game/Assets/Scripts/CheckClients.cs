using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Net;

public class CheckClients : MonoBehaviour
{

    public int timeToDisc = 1000;
    public static Dictionary<string, long> lastAlive; // device identifier => ticks on last ping
    public int pingsPort = 3003;

    private Thread listener, reporter;

    // Use this for initialization
    void Start()
    {

        lastAlive = new Dictionary<string, long>();

        listener = new Thread(new ThreadStart(listenForPings));
        reporter = new Thread(new ThreadStart(reportConnectivity));

        try
        {
            listener.Start();
            reporter.Start();
        }
        catch (ThreadInterruptedException e)
        {
            Debug.Log("Thread was interrupted");
            Debug.Log(e);
        }

    }

    void OnDestroy()
    {

        listener.Abort();
        listener.Join();

        reporter.Abort();
        reporter.Join();
    }

    void listenForPings()
    {

        while (true)
        {
            if (lastAlive.Count != 0)
            {
                Message msg = NetworkHandler.receiveMessage(pingsPort, timeToDisc);

                if (msg.body != "")
                {
                    //Debug.Log("Ping received on " + (float)DateTime.Now.Ticks);
                    lock (lastAlive)
                    {
                        lastAlive[msg.body] = DateTime.Now.Ticks;
                    }
                }
            }
            else
            {
                //Debug.Log("No players Found");
            }
            Thread.Sleep(1);
        }
    }

    void reportConnectivity()
    {
        TimeSpan allowed = new TimeSpan(timeToDisc * TimeSpan.TicksPerMillisecond);
        List<string> keysToRemove = new List<string>();
        //Debug.Log("Ticks To Disconnect = " + ticksToDisc);

        while (true)
        {
            //Debug.Log("Checking list");
            lock (lastAlive)
            {
                foreach (KeyValuePair<string, long> entry in lastAlive)
                {
                    //Debug.Log("Last ping was " + (DateTime.Now.Ticks - entry.Value) + " ticks ago. Ticks To Disc = " + ticksToDisc);
                    TimeSpan span = new TimeSpan(DateTime.Now.Ticks - entry.Value);

                    Debug.Log("Client " + entry.Key + " span = " + span + ". Allowed = " + allowed);
                    if (span > allowed)
                    {
                        Debug.Log("Client " + entry.Key + " disconnected");
                        keysToRemove.Add(entry.Key);
                    }
                    else
                    {
                        Debug.Log("Client " + entry.Key + " is alive");
                    }
                }

                foreach (string key in keysToRemove)
                {
                    lastAlive.Remove(key);
                }
                keysToRemove.Clear();
            }

            Debug.Log(lastAlive.Count + " client(s) are alive");
            Thread.Sleep(timeToDisc);
        }
    }



}
