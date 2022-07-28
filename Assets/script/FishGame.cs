using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;



using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class FishGame
{
    static readonly string Turn = "turn";
    static Hashtable property = new Hashtable();

    public static void SetTurn(this Room room ,int _turn)
    {
        property[Turn] = _turn;
        room.SetCustomProperties(property);
        property.Clear();
    }

    public static int GetTurn(this Room room)
    {
        return room.CustomProperties[Turn] is int turn ? turn : -1;
    }

    

}
