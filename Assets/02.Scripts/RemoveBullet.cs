using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    public GameObject sparkEffect;
    
    private void OnCollisionEnter(Collision collision)
    {
        // garbage collection is activated use comparetag instead
       if (collision.collider.tag == "BULLET")
        {
            Destroy(collision.gameObject);
            // 스파크 이팩트, 위치, 법선 벡터
            Vector3 pos = collision.GetContact(0).point;
            // Quaternion 각도단위
            Vector3 normal = -collision.GetContact(0).normal;

            // 벡터가 바라보는 각도의 산출
            Quaternion rot = Quaternion.LookRotation(normal);
            var gobj = Instantiate(sparkEffect, pos, rot);
            Destroy(gobj, 0.5f);
        }
    }

    //// 충돌후 붙어 있는경우
    //private void OnCollisionStay(Collision collision)
    //{
        
    //}

    //// 충돌후 떨어지면
    //private void OnCollisionExit(Collision collision)
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
