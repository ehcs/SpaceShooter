using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene ȣ��

public class UIManager : MonoBehaviour
{
    public void onStartButtonClick()
    {
        //Debug.Log("��ư Ŭ��");
        SceneManager.LoadScene(2);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
