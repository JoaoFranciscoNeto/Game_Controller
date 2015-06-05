using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json;

public class Drag : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    private Vector2 initPos;

    public void BeginSwipe(BaseEventData data)
    {
        PointerEventData d = data as PointerEventData;
        initPos = d.position;

        Debug.Log("Init Pos = " + initPos);
    }

    public void EndSwipe(BaseEventData data)
    {
        PointerEventData d = data as PointerEventData;

        Play p = new Play();
        p.jump = false;
        p.move = d.position - initPos;


        Debug.Log("Play = " + p.move);

        Play_Object pl = new Play_Object(p, ApplicationModel.identifier);

        string msg = JsonConvert.SerializeObject(pl, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        HTTP.Request request = new HTTP.Request("post", "http://" + ApplicationModel.serverAddr + ":2225/game/", bytes);
        request.Send();
    }
}
