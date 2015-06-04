using UnityEngine;
using System.Collections;

public class Player {

    public string uniqueID;
    public string userName;

    
    public override string ToString () {
        return "Player User Name = " + userName + " from device " + uniqueID;
    }
}
