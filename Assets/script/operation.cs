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
    public Text text;
    Text Wintext;

    bool GameOver = false;
    public bool Win = true;
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
        Wintext = GameObject.Find("Win").GetComponent<Text>();
        text = GameObject.Find("turn").GetComponent<Text>();
        FALL = GameObject.Find("Fall");
        Fall = FALL.GetComponent<fall>();

        if (!photonView.IsMine)
            return;

        photonView.RPC(nameof(CreatRandomFish), RpcTarget.All, player.ActorNumber, 1);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
             return;
        }

        if (!photonView.IsMine)
            return;

        int acterNumber = player.ActorNumber;

        if (room.GetTurn() != acterNumber - 1)
        {
            text.text = "相手のターン";
          
            return;
        }
        

        text.text = "自分のターン";
        if (Fall.active == false)
        {
            Fall.Timer += Time.deltaTime;
            if (Fall.Timer > Fall.RespownTime)
            {
                Fall.Timer = 0;
                
                photonView.RPC(nameof(CreatRandomFish), RpcTarget.All, player.ActorNumber, Random.Range(0, Fall.Train.Length - 1));

                for (int i = 0; i < Fall.FishList.Count; i++)
                {
                    photonView.RPC(nameof(PositionReset), RpcTarget.All, Fall.FishList[i].transform.position, Fall.FishList[i].transform.rotation, i, player.ActorNumber);
                }
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
            //photonView.RPC(nameof(RotationSync), RpcTarget.Others, Fall.fish.transform.rotation, player.ActorNumber);
        }
        Fall.buttomup = false;
        if (Fall.fish.transform.position.y <= -6)
        {
            photonView.RPC(nameof(GameEnd), RpcTarget.All, player.ActorNumber);
          
        }

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

    [PunRPC]
    void GameEnd(int _ActorNumber)
    {
        Wintext.text = "プレイヤー" + _ActorNumber + "の負け";
        GameOver = true;
        if (player.ActorNumber != _ActorNumber)
            return;
        Win = false;

    }

    [PunRPC]
    void RotationSync(Quaternion Rotation, int _ActorNumber )
    {
        if (player.ActorNumber != _ActorNumber)
            return;
        Fall.fish.transform.rotation = Rotation;

    }

    [PunRPC]
    void PositionReset(Vector3 pos ,Quaternion Rotation, int number , int _ActorNumber)
    {
        if (player.ActorNumber != _ActorNumber)
            return;

        Fall.FishList[number].transform.position = pos;
        Fall.FishList[number].transform.rotation = Rotation;
    }
}
