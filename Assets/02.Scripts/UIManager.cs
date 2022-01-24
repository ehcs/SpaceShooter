using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 호출

public class UIManager : MonoBehaviour
{
    public void onStartButtonClick()
    {
        //Debug.Log("버튼 클릭");
        SceneManager.LoadScene(2);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
