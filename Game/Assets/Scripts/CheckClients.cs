using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Net;
using System.Text.RegularExpressions;

public class CheckClients : MonoBehaviour
{

    public int timeToDisc = 1000;
    public static Dictionary<string, long> lastAlive; // device identifier => ticks on last ping
    public int pingsPort = 3003;

    private string pingPattern = @"ping;(\w+)";
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

            //Debug.Log("Listening with player count = " + lastAlive.Count);

            Message msg = NetworkHandler.receiveMessage(pingsPort, timeToDisc);
            //Debug.Log("Message " + msg.body + "  on " + DateTime.Now.Ticks);
            if (Regex.IsMatch(msg.body,pingPattern))
            {
                string id = Regex.Match(msg.body, pingPattern).Groups[1].Value;
                lock (lastAlive)
                {
                    lastAlive[id] = DateTime.Now.Ticks;
                }
            }
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
