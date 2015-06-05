using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;

    private Dictionary<string, PlayerMovement> playerMovements; // id => playerMovement

    private Queue<Play_Object> plays;

	// Use this for initialization
	void Start () {

        plays = new Queue<Play_Object>();

        GameObject.FindGameObjectWithTag("Permanent").GetComponent<Server_Listener>().setManager(this);

        playerMovements = new Dictionary<string, PlayerMovement>();

        foreach (Player p in ApplicationModel.controllers.Values)
        {
            GameObject m = Instantiate(playerPrefab, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
            playerMovements[p.uniqueID] = m.GetComponent<PlayerMovement>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        lock (plays)
        {
            foreach (Play_Object obj in plays)
            {
                playerMovements[obj.playerID].Move(obj.play.move, obj.play.jump);
            }

            plays.Clear();
        }
	}

    public void addPlay(Play_Object obj)
    {
        lock (plays)
        {
            plays.Enqueue(obj);
        }
    }

}
