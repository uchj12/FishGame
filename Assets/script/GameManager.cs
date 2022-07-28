using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    Room room;
    private void Awake()
    {
        room = PhotonNetwork.CurrentRoom;
        if (PhotonNetwork.IsMasterClient)
            room.SetTurn(0);

        PhotonNetwork.Instantiate("PlayerController", Vector3.zero, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ルームプロパティーが更新されたときに呼ばれるコールバック
    /// </summary>
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        foreach (var prop in propertiesThatChanged)
        {
            Debug.Log($"{prop.Key}: {prop.Value}");
        }
    }

}
