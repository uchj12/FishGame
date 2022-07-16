using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class fall : MonoBehaviour
{
   
    public GameObject[] Train;
    public GameObject fish;
    public GameObject camera;
    public List<GameObject> FishList = new List<GameObject>();
    public GameObject Shark;
   
    public score scoreManager;
    
    bool Move = true;
    Vector2 MousePosition;
    bool downflag = false;
    int number;
    public bool active = true;
    float Timer = 0.0f;
    float RespownTime = 3.0f;
    bool Play = true;

    private float Distance = 0;
    Vector3 oldpos;
   
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        create();//���̐���
        oldpos = fish.transform.position;

       
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Play == true)
        {
            if (active == false)
            {
                Timer += Time.deltaTime;
                if (Timer > RespownTime)
                {
                    if (fish.transform.position.y >= 1.0f)//���̍��W�������Ȃ�����J�����̈ړ�
                    {
                        camera.transform.position = new Vector3(0.0f, fish.transform.position.y - 1.0f, -10.0f);
                    }
                    Timer = 0.0f;
                    scoreManager.Addscore();//�X�R�A�̉��Z
                    create();//���̐���
                }
            }
            if (downflag == true && active == true)
            {
                fish.transform.Rotate(new Vector3(0, 0, -1));
            }
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())//�{�^���������Ă��Ȃ��A�}�E�X�N���b�N�ŋ��̈ړ�
            {
                if (Move == true)
                {
                    MovePiece();
                }
            }
        }
    }
    
    public void OnButtonDown()
    {
        downflag = true;
        oldpos = new Vector3(0,0,0);
    }

    public void OnButtonUp()
    {
  
        Move = false;
        downflag = false;
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;//���̍��W�Œ������
        active = false;
        oldpos = fish.transform.position;

        GetComponent<AudioSource>().Play();  // ���ʉ���炷
        //Impairing();
        FishList.Add(fish);//List�ɋ����i�[
    }

    void create()//���̐���
    {
        active = true;
        Move = true;

            number = Random.Range(0, Train.Length - 1);//�����_���ɐ���
        

      
        fish = Instantiate(Train[number], new Vector3(0.0f, camera.transform.position.y + 3.0f, 0.0f), transform.rotation);
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;//���̍��W�𓮂��Ȃ��悤�ɌŒ�

        if (FishList.Count > 3)
        {
            //float rnd = Random.Range(0, 10);//�P�O���̈�̊m���Ő���
            //if (rnd > 1)
            //{
            //    Delete_Shark();
            //}
        }
    }

    void MovePiece()//���𓮂���
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fish.transform.position = new Vector2 (MousePosition.x, camera.transform.position.y + 3.0f);
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

    void Impairing()//�����_���ŋ����P�O�C����
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
}
