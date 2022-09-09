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
public class fall : MonoBehaviour
{
    public GameObject[] Train;
    public GameObject fish;
    public GameObject camera;
    public List<GameObject> FishList = new List<GameObject>();
    public GameObject Shark;  
    public bool Move = true;
    public bool downflag = false;
    int number;
    public bool active = true;
    public bool buttomup = false;
    public float Timer = 0.0f;
    public float RespownTime = 3.0f;
    bool Play = true;

    private float Distance = 0;
    Vector3 oldpos;
   
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        //create();//���̐���
        oldpos = fish.transform.position;
        camera = GameObject.Find("Main Camera");
       
    }

    // Update is called once per frame
    void Update()
    {


    }
    
    public void OnButtonDown()
    {
        downflag = true;
        oldpos = new Vector3(0,0,0);
    }

    public void OnButtonUp()
    {
       
        buttomup = true;
        Move = false;
        downflag = false;
        active = false;
        oldpos = fish.transform.position;

        GetComponent<AudioSource>().Play();  // ���ʉ���炷
        //Impairing();
        FishList.Add(fish);//List�ɋ����i�[

    }

    public void create(int _number)//���̐���
    {
        active = true;
        Move = true;
        //number = Random.Range(0, Train.Length - 1);//�����_���ɐ���
        number = _number;      

        fish = Instantiate(Train[number], new Vector3(0.0f, camera.transform.position.y + 3.0f, 0.0f), Quaternion.identity);
        
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;//���̍��W�𓮂��Ȃ��悤�ɌŒ�
    }

    public void MovePiece(float MousePosX)//���𓮂���
    {
      
        fish.transform.position = new Vector2 (MousePosX, camera.transform.position.y + 3.0f);
    }


    void Delete_Shark()//�T�����o�Ă��ċ��i�s�[�X�j�H�ׂ�i�����j
    {
        Play = false;//�v���C���~
        Shark.transform.position = new Vector3(4,5,0);
        number = Random.Range(0, FishList.Count);//��ɂ��鋛�̒����烉���_���ɑI��

        Shark.transform.DOJump(//���̏ꏊ�܂ňړ�
           FishList[number].transform.position,
            2.0f,
            2,
            1.5f).OnComplete(() =>//�ړ������������狛�������A�v���C���ĊJ
            {
                Destroy(FishList[number]);
                FishList.RemoveAt(number);
                Shark.transform.position = new Vector3(4, 5, 0);
                Play = true;
                
            });
    }

    public void Impairing()//�����_���ŋ����P�O�C����
    {
        float rnd = Random.Range(0, 10);//�P�O���̈�̊m���Ő���
        if (rnd == 1)
        {
            for (float i = 0; i < 10; i++)
            {
                rnd = Random.Range(-2.0f, 2.0f);//�o������ꏊ��x�����������_���Ɂ@�@y���͋��𐶐����邽�тɍ�����ς���
                number = 5;
                fish = Instantiate(Train[number], new Vector3(rnd, camera.transform.position.y + 5.0f + i * 0.5f, 0.0f), transform.rotation);
            }
        }
    }

    public void FishRelease()
    {
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;//���̍��W�Œ������
        fish.transform.rotation = fish.transform.rotation;
    }
}
