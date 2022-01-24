using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Transform[] points;
    public GameObject monsterPrefab;
    public List<GameObject> monsterPool = new List<GameObject>();

    public int maxPool = 20; 
    public float createTime = 3.0f;
    public bool isGameOver = false;

    public TMP_Text scoreText;

    private float score;
    public float Score
    {
        get { return score; }
        set
        {
            Debug.Log(value);
            Debug.Log(score);
            score += value;
            Debug.Log(score);
            scoreText.text = $"<color=#00ff00>Score</color> : <color=#ff0000>{score:00000}</color>";
            PlayerPrefs.SetFloat("SCORE", score);
        }
    }


    private WaitForSeconds ws;
       
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += GameOver;
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= GameOver;
    }

    private void GameOver()
    {
        isGameOver = true;
    }

    private void CreatePooling()
    {
        for (int i = 0; i < maxPool; i++)
        {
            GameObject monster = Instantiate<GameObject>(monsterPrefab);
            monster.name = $"Monster_{i:00}";
            monster.SetActive(false);

            monsterPool.Add(monster);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Score = PlayerPrefs.GetFloat("SCORE", 0.0f);

        ws = new WaitForSeconds(createTime);

        points = GameObject.Find("SPAWNPOINTGROUP")?.GetComponentsInChildren<Transform>();
        
        CreatePooling();

        //InvokeRepeating("CreateMonster", 2.0f, createTime);
        StartCoroutine(CreateMonster());
    }

    //void CreateMonster()
    //{
    //    if (isGameOver) CancelInvoke(); // Invoke 로 만들어진 경우

    //    int idx = Random.Range(1, points.Length);
    //    Instantiate(monsterPrefab, points[idx].position, Quaternion.identity);
    //}

    IEnumerator CreateMonster()
    {
        yield return new WaitForSeconds(2.0f);

        while (!isGameOver)
        {
            foreach (var monster in monsterPool)
            {
                if (monster.activeSelf == false)
                {
                    int idx = Random.Range(1, points.Length);
                    monster.transform.position = points[idx].position;
                    monster.transform.rotation = Quaternion.LookRotation(points[0].position - points[idx].position); // vector A - B = direction 
                    monster.SetActive(true);
                    break;
                }
            }
            
            //Instantiate(monsterPrefab, points[idx].position, Quaternion.identity);


            yield return ws;
        }        
    }

    
}
