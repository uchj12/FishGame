using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class operation : MonoBehaviourPunCallbacks
{
    static operation MyInstance;
    private fall Fall;
    GameObject FALL;

    Room room;
    Player player;
    Vector3 MousePosition;
    private void Awake()
    {
        room = PhotonNetwork.CurrentRoom;


        if (photonView.IsMine)
        {
            player = PhotonNetwork.LocalPlayer;
            MyInstance = this;
        }
        else {
            player = photonView.Owner;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FALL = GameObject.Find("Fall");
        Fall = FALL.GetComponent<fall>();

        if (!photonView.IsMine)
            return;

        photonView.RPC(nameof(CreatRandomFish), RpcTarget.All, player.ActorNumber, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

          
        int acterNumber = player.ActorNumber;

        if (room.GetTurn() != acterNumber - 1)
            return;

       
        if (Fall.active == false)
        {
            Fall.Timer += Time.deltaTime;
            if (Fall.Timer > Fall.RespownTime)
            {
                Fall.Timer = 0;
                photonView.RPC(nameof(CreatRandomFish), RpcTarget.All, player.ActorNumber, Random.Range(0, Fall.Train.Length - 1));
            }
        }


        if (Fall.downflag == true && Fall.active == true)
        {
            photonView.RPC(nameof(PlayerRotation), RpcTarget.All, new Vector3(0, 0, 1), player.ActorNumber);
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())//ボタンを押していない、マウスクリックで魚の移動
        {

            if (Fall.Move == true)
            {
                MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                photonView.RPC(nameof(PlayerMove), RpcTarget.All, MousePosition.x, player.ActorNumber);
            }
        }
        if (Fall.buttomup == true)
        {
            photonView.RPC(nameof(PlayerRelease), RpcTarget.All,player.ActorNumber);
        }
        Fall.buttomup = false;
    }

    [PunRPC]
    void CreatRandomFish(int _ActorNumber, int _number)
    {
        if (player.ActorNumber != _ActorNumber)
            return;

        Fall.create(_number);

        room.SetTurn((room.GetTurn() + 1) % 2);
    }


    [PunRPC]
    void PlayerRotation(Vector3 Rotation,int _ActorNumber)
    {
        if (player.ActorNumber != _ActorNumber)
            return;
        Fall.fish.transform.Rotate(Rotation);
    }

    [PunRPC]
    void PlayerMove(float MousePosX, int _ActorNumber)
    {
        if (player.ActorNumber != _ActorNumber)
            return;
        Fall.MovePiece(MousePosX);
    }

    [PunRPC]
    void PlayerRelease(int _ActorNumber)
    {
        if (player.ActorNumber != _ActorNumber)
            return;
        Fall.FishRelease();
    }

}
