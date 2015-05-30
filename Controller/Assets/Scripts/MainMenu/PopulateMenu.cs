using UnityEngine;
using System.Collections;

public class PopulateMenu : MonoBehaviour {

    public GameObject sampleButton;

    public Transform parentContainer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            addButton();
        }
	
	}

    private void addButton()
    {
        GameObject newButton = Instantiate(sampleButton) as GameObject;

        newButton.transform.SetParent(parentContainer);
    }
}
