using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChange : MonoBehaviour
{
    public GameObject fish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fish.transform.position.y < -10.0f)
        {
            SceneManager.LoadScene("GameOver");
        }

    

    }

     
    

}