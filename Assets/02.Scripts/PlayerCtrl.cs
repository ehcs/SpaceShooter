using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f, v = 0.0f, r = 0.0f;

    [Range(5.0f, 20.0f)]
    public float moveSpeed = 8.0f;

    [Range(100.0f, 2000.0f)]
    public float turnSpeed = 200.0f;
    private float _turnSpeed;

    private float initHp = 100.0f;
    public float curHp = 100.0f;

    public Image hp;

    //[HideInInspector]
    [System.NonSerialized]
    public Animation anim;

    public delegate void PlayerDieHandler(); // 함수 호출자 타입
    public static event PlayerDieHandler OnPlayerDie;

    //private void Awake()
    //{
        // runs even when the obejct is diabled
    //}

    //private void OnEnable()
    //{
        
    //}

    // Start is called before the first frame update
    IEnumerator Start() // 초기 Garbage값 반영 안하는 방법
    {
        _turnSpeed = 0.0f;

        anim = GetComponent<Animation>();
        anim.Play("Idle");

        yield return new WaitForSeconds(0.5f);

        _turnSpeed = turnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // 오차발생 가능
        //transform.position += new Vector3(0, 0, 0.1f);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 dir = Vector3.forward * v + Vector3.right * h;        
        //transform.Translate(Vector3.forward * 0.1f);
        //transform.Translate(Vector3.forward * v);
        //transform.Translate(Vector3.right * h);

        //Debug.Log("dir = " + dir.magnitude + " normalized=" + dir.normalized.magnitude);

        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed); // normalized = 1

        transform.Rotate(Vector3.up * Time.deltaTime * _turnSpeed * r);

        //Debug.Log("r= " + r);
        /* Normalized Vector, Unit Vector
            Vector3.forward = Vector3(0, 0, 1)
            Vector3.up      = Vector3(0, 1, 0)
            Vector3.right   = Vector3(1, 0, 0)
            Vector3.one  = Vector3(1, 1, 1)
            Vector3.zero = Vector3(0, 0, 0)
        */

        //Debug.Log("h=" + h + "v=" + v);
        PlayerAnim();
    }

    void PlayerAnim()
    {
        if (v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.3f);
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.3f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.3f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.3f);
        }
        else
        {
            anim.CrossFade("Idle", 0.3f);
        }
    }

    private void FixedUpdate()
    {
        // 균일한 0.02초
        // 물리엔진 시뮬레이션 
    }

    private void LateUpdate()
    {
        // 60번 호출
        // 후처리
        // follow camera 
    }

    // OnCollision (Enter, Stay, Exit)
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("PUNCH"))
        //{
        //    curHp -= 10.0f;
        //    if (curHp <= 0.0f) // && other.CompareTag("PUNCH")
        //    {
        //        PlayerDie();
        //    }
        //}        
        if (curHp > 0.0f && other.CompareTag("PUNCH"))
        {
            curHp -= 10.0f;
            hp.fillAmount = curHp / initHp;
            if (curHp <= 0.0f)
            {
                PlayerDie();
            }
        }

        void PlayerDie()
        {
            Debug.Log("주인공 사망");

            //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
            //foreach (var monster in monsters)
            //{
            //    //monster.GetComponent<MonsterCtrl>().YouWin();

            //    monster.SendMessage("YouWin", SendMessageOptions.DontRequireReceiver); // YouWin이라는 함수 실행
            //}

            OnPlayerDie();
        }
    }
}
