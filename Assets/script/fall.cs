using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using DG.Tweening;
public class fall : MonoBehaviour
{
    public GameObject[] Train;
    public GameObject fish;
    public GameObject camera;
   
    public score scoreManager;
    bool Move = true;
    Vector2 MousePosition;
    bool downflag = false;
    int number;
    public bool active = true;

    private float Distance = 0;
    Vector3 oldpos;
   
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        create();
        oldpos = fish.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
            Distance = Vector2.Distance(oldpos, fish.transform.position);//‘O‚ÌÀ•W‚Æ¡‚ÌÀ•W‚Ì‹——£
          


            if (Distance >= 3 && active == false && fish.GetComponent<Rigidbody2D>().IsSleeping())
            {
                if (fish.transform.position.y >= 1.0f)
                {
                    camera.transform.position = new Vector3(0.0f, fish.transform.position.y - 1.0f, -10.0f);
                }
            scoreManager.Addscore();
            create();
     
        }
        if (downflag == true && active == true)
        {
            fish.transform.Rotate(new Vector3(0,0,-1));
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Move == true)
            {
                MovePiece();
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
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        active = false;
        oldpos = fish.transform.position;

        GetComponent<AudioSource>().Play();  // Œø‰Ê‰¹‚ð–Â‚ç‚·
        float rnd = Random.Range(0, 10);
        if (rnd == 1)
        {
            for (float i = 0; i < 10; i++)
            {
                rnd = Random.Range(-2.0f, 2.0f);
                number = 5;
                fish = Instantiate(Train[number], new Vector3(rnd, camera.transform.position.y + 5.0f + i * 0.5f, 0.0f), transform.rotation);
            }
        }
    }

    void create()
    {
        active = true;
        Move = true;
        number = Random.Range(0, Train.Length - 1);
        fish = Instantiate(Train[number], new Vector3(0.0f, camera.transform.position.y + 3.0f, 0.0f), transform.rotation);
        fish.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

    }

    void MovePiece()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fish.transform.position = new Vector2 (MousePosition.x, camera.transform.position.y + 3.0f);
    }

    
}
