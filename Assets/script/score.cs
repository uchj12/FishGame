using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class score : MonoBehaviour
{
    public int scores;
    public int basicScore = 10;
    // Start is called before the first frame update
    void Start()
    {
        scores = 0;
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "score:" + scores;
    }
    public void Addscore()
    {
        scores +=basicScore;
    }
}