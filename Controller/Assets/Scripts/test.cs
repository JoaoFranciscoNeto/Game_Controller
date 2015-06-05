using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class test : MonoBehaviour {

    public List<Button> buttons;
    public List<Button> touchPads;

    private bool[] buttonBools;
    
	// Use this for initialization
	void Start () {
        buttonBools = new bool[buttons.Count];
        initArray();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void initArray()
    {
        for (int i = 0; i < buttonBools.Length; i++)
        {
            buttonBools[i] = false;
        }
    }

    public void Send(int button)
    {
        buttonBools[button] = true;

        for (int i = 0; i < buttonBools.Length; i++)
        {
            Debug.Log(buttonBools[i]);
        }
        // percorrer buttoes para input
        // enviar codificado em json

        initArray();
    }

    public void tt()
    {
        Debug.Log("Began swipe");
    }

    public void tte()
    {
        Debug.Log("End swipe");
    }

    public void tt(CNT_Button b)
    {
        Debug.Log(b.identifier);
    }
}
