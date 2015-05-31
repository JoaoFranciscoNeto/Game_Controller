using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class CheckClients : MonoBehaviour {

    public int timeToDisc = 1000;
    Dictionary<string, float> lastAlive;

	// Use this for initialization
	void Start () {

        lastAlive = new Dictionary<string, float>();

        Thread listener = new Thread(new ThreadStart(listenForPings));
        Thread reporter = new Thread(new ThreadStart(reportConnectivity));

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

    void listenForPings()
    {
        while (true)
        {
            Message msg = NetworkHandler.receiveMessage(2225);

            lock (lastAlive)
            {
                lastAlive[msg.body] = Time.time;
            }
        }
    }

    void reportConnectivity()
    {
        List<string> keysToRemove = new List<string>();
        lock (lastAlive)
        {
            
            foreach (KeyValuePair<string, float> entry in lastAlive)
            {
                if (Time.time - entry.Value > timeToDisc)
                {
                    Debug.Log("Client " + entry.Key + " disconnected");
                    keysToRemove.Add(entry.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                lastAlive.Remove(key);
            }
        }

        Thread.Sleep(timeToDisc);
    }



}
