using UnityEngine;
using System.Collections;
using System;

public class Play_Object
{

    public Play play;
    public String playerID;

    public Play_Object(Play _play, String _playerID)
    {
        play = _play;
        playerID = _playerID;
    }
}