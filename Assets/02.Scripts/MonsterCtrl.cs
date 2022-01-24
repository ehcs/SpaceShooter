using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // NavMesh 이용시 추가

public class MonsterCtrl : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK, 
        DIE
    }

    public State state = State.IDLE;

    private Transform playerTr;
    private Transform monsterTr;
    private NavMeshAgent agent;
    private Animator anim;
    private float HP = 100.0f;

    //[SerializeField]
    private GameObject bloodEffect;
    //private GameManager gameManager;

    public float attackDist = 2.0f;
    public float traceDist = 10.0f;

    public bool isDead = false;
    // parameter 해시값 추출
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashAniSpeed = Animator.StringToHash("AniSpeed");
        
    // event 연결때 사용
    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.YouWin;
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.YouWin;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //GameObject playerObj = GameObject.FindGameObjectWithTag("PLAYER");
        //if (playerObj != null)
        //{
        //    playerTr = playerObj.GetComponent<Transform>();
        //}

        playerTr = GameObject.FindGameObjectWithTag("PLAYER")?.GetComponent<Transform>();
        monsterTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // dynamic loading
        bloodEffect = Resources.Load<GameObject>("BloodEffect"); // "Prefabs/BloodEFfect"

        //StartCoroutine(CheckMonsterState());
        //StartCoroutine(MonsterAction());

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator CheckMonsterState()
    {
        while (!isDead)
        {
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            } else if (distance <= traceDist) {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

            yield return new WaitForSeconds(0.3f);                        
        }
    }


    IEnumerator MonsterAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool(hashTrace, false); // "IsTrace"
                    //agent.isStopped = true;
                    break;
                case State.TRACE:
                    anim.SetBool(hashAttack, false);
                    anim.SetBool(hashTrace, true);
                    agent.isStopped = false;
                    agent.SetDestination(playerTr.position);
                    break;
                case State.ATTACK:
                    //anim.SetBool(hashTrace, false);
                    anim.SetBool(hashAttack, true);
                    agent.isStopped = true;
                    break;
                case State.DIE:
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET")) {            
            Destroy(collision.gameObject);
            //anim.SetTrigger(hashHit);
            //Vector3 pos = collision.GetContact(0).point;
            //Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            //GameObject blood = Instantiate(bloodEffect, pos, rot);
            //Destroy(blood, 0.5f);

            //HP -= 25.0f;
            //if (HP <= 0.0f)
            //{
            //    MonsterDie();   
            //}
        }
    }

    public void OnDamage(Vector3 pos, Vector3 normal, float damage)
    {
        anim.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 0.5f);

        HP -= damage;
        if (HP <= 0.0f)
        {
            MonsterDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    void MonsterDie()
    {
        Debug.Log("Moster Die!");

        //gameManager.score += 50.0f;
        GameManager.instance.Score = 50.0f;
        // 네비 정지
        agent.isStopped = true;

        // Die anim 실행
        anim.SetTrigger(hashDie);

        GetComponent<CapsuleCollider>().enabled = false;

        // 코루틴 종료
        StopAllCoroutines();

        Invoke("ReturnPooling", 5.0f);
        //Destroy(this.gameObject, 5.0f);
    }

    void ReturnPooling()
    {
        this.gameObject.SetActive(false);

        isDead = false;
        HP = 100.0f;
        GetComponent<CapsuleCollider>().enabled = true;
        state = State.IDLE;

    }

    public void YouWin()
    {
        anim.SetFloat(hashAniSpeed, Random.Range(0.8f, 1.2f));
        agent.isStopped = true;
        StopAllCoroutines();
        anim.SetTrigger(hashPlayerDie);
    }    
}
