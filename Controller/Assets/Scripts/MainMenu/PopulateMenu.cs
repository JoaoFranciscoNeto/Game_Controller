using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        SampleButton button = newButton.GetComponent<SampleButton>();

        button.nameLabel.text = "Nome do Servidor";
        button.ipLabel.text = "192.168.0.1";
        newButton.transform.SetParent(parentContainer,false);

    }
}
