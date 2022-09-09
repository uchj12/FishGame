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
        //create();//魚の生成
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

        GetComponent<AudioSource>().Play();  // 効果音を鳴らす
        //Impairing();
        FishList.Add(fish);//Listに魚を格納

    }

    public void create(int _number)//魚の生成
    {
        active = true;
        Move = true;
        //number = Random.Range(0, Train.Length - 1);//ランダムに生成
        number = _number;      

        fish = Instantiate(Train[number], new Vector3(0.0f, camera.transform.position.y + 3.0f, 0.0f), Quaternion.identity);
        
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;//魚の座標を動かないように固定
    }

    public void MovePiece(float MousePosX)//魚を動かす
    {
      
        fish.transform.position = new Vector2 (MousePosX, camera.transform.position.y + 3.0f);
    }


    void Delete_Shark()//サメが出てきて魚（ピース）食べる（消す）
    {
        Play = false;//プレイを停止
        Shark.transform.position = new Vector3(4,5,0);
        number = Random.Range(0, FishList.Count);//場にある魚の中からランダムに選択

        Shark.transform.DOJump(//魚の場所まで移動
           FishList[number].transform.position,
            2.0f,
            2,
            1.5f).OnComplete(() =>//移動が完了したら魚を消し、プレイを再開
            {
                Destroy(FishList[number]);
                FishList.RemoveAt(number);
                Shark.transform.position = new Vector3(4, 5, 0);
                Play = true;
                
            });
    }

    public void Impairing()//ランダムで魚を１０匹生成
    {
        float rnd = Random.Range(0, 10);//１０分の一の確率で生成
        if (rnd == 1)
        {
            for (float i = 0; i < 10; i++)
            {
                rnd = Random.Range(-2.0f, 2.0f);//出現する場所をx軸だけランダムに　　y軸は魚を生成するたびに高さを変える
                number = 5;
                fish = Instantiate(Train[number], new Vector3(rnd, camera.transform.position.y + 5.0f + i * 0.5f, 0.0f), transform.rotation);
            }
        }
    }

    public void FishRelease()
    {
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;//魚の座標固定を解除
        fish.transform.rotation = fish.transform.rotation;
    }
}
