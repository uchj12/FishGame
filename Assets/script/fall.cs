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
        create();//魚の生成
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
                    if (fish.transform.position.y >= 1.0f)//魚の座標が高くなったらカメラの移動
                    {
                        camera.transform.position = new Vector3(0.0f, fish.transform.position.y - 1.0f, -10.0f);
                    }
                    Timer = 0.0f;
                    scoreManager.Addscore();//スコアの加算
                    create();//魚の生成
                }
            }
            if (downflag == true && active == true)
            {
                fish.transform.Rotate(new Vector3(0, 0, -1));
            }
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())//ボタンを押していない、マウスクリックで魚の移動
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
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;//魚の座標固定を解除
        active = false;
        oldpos = fish.transform.position;

        GetComponent<AudioSource>().Play();  // 効果音を鳴らす
        //Impairing();
        FishList.Add(fish);//Listに魚を格納
    }

    void create()//魚の生成
    {
        active = true;
        Move = true;

            number = Random.Range(0, Train.Length - 1);//ランダムに生成
        

      
        fish = Instantiate(Train[number], new Vector3(0.0f, camera.transform.position.y + 3.0f, 0.0f), transform.rotation);
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;//魚の座標を動かないように固定

        if (FishList.Count > 3)
        {
            //float rnd = Random.Range(0, 10);//１０分の一の確率で生成
            //if (rnd > 1)
            //{
            //    Delete_Shark();
            //}
        }
    }

    void MovePiece()//魚を動かす
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fish.transform.position = new Vector2 (MousePosition.x, camera.transform.position.y + 3.0f);
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

    void Impairing()//ランダムで魚を１０匹生成
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
}
