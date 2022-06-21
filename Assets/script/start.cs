using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("main");
    }
}
