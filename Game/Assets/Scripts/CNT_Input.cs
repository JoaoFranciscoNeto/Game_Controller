using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CNT_Input : MonoBehaviour {

    private enum BTN_STATE
    {
        UP,
        DOWN
    }

    public List<string> buttonNames;

    private Dictionary<string, BTN_STATE> buttons;

	// Use this for initialization
	void Start () {
        foreach (string name in buttonNames)
        {
            buttons[name] = BTN_STATE.UP;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Input.GetButton("");
	}

    
}
