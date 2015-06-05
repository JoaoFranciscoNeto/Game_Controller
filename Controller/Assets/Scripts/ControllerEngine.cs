using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Newtonsoft.Json;


public class ControllerEngine : MonoBehaviour {

    List<CNT_Button> buttons;

	// Use this for initialization
    void Start()
    {
        
        buttons = new List<CNT_Button>(GameObject.FindObjectsOfType<CNT_Button>());
        Debug.Log("There are " + buttons.Count + " buttons in the scene");

        foreach (CNT_Button btn in buttons)
        {
            EventTrigger trigger = btn.GetComponent<EventTrigger>();

            //AddEventTrigger(trigger, testFunction, EventTriggerType.PointerUp);
            AddEventTrigger(trigger, OnPointerClick, EventTriggerType.PointerDown);
        }
        
	}

    #region TriggerEvents

    private void AddEventTrigger(EventTrigger trigger, UnityAction action, EventTriggerType triggerType)
    {
        // Create the trigger
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        triggerEvent.AddListener((eventData) => action());

        // Create the entry
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = triggerType };

        // Add the entry
        trigger.delegates.Add(entry);
    }

    private void AddEventTrigger(EventTrigger trigger, UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        triggerEvent.AddListener((eventData) => action(eventData)); // you can capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        trigger.delegates.Add(entry);
    }

    #endregion
    
    private void OnPointerClick(BaseEventData data)
    {
        Play p = new Play();
        p.jump = true;
        p.move = new Vector2(0, 0);

        Play_Object pl = new Play_Object(p, ApplicationModel.identifier);

        string msg = JsonConvert.SerializeObject(pl, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        HTTP.Request request = new HTTP.Request("post", "http://" + ApplicationModel.serverAddr + ":2225/game/", bytes);
        request.Send();
    }
}
