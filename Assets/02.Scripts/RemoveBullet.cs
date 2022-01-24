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
            // ����ũ ����Ʈ, ��ġ, ���� ����
            Vector3 pos = collision.GetContact(0).point;
            // Quaternion ��������
            Vector3 normal = -collision.GetContact(0).normal;

            // ���Ͱ� �ٶ󺸴� ������ ����
            Quaternion rot = Quaternion.LookRotation(normal);
            var gobj = Instantiate(sparkEffect, pos, rot);
            Destroy(gobj, 0.5f);
        }
    }

    //// �浹�� �پ� �ִ°��
    //private void OnCollisionStay(Collision collision)
    //{
        
    //}

    //// �浹�� ��������
    //private void OnCollisionExit(Collision collision)
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
