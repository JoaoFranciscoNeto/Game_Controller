using UnityEngine;
using System.Collections;
using System.Threading;

public class ControllerNetwork : MonoBehaviour {
    private Thread pinger;
	// Use this for initialization
	void Start () {
        pinger = new Thread(() => KeepAlive.sendPings(ApplicationModel.serverAddr));
        pinger.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy ()
    {
        pinger.Abort();
        pinger.Join();
    }
}
