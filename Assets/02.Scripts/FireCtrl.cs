using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePos;
    public AudioClip fireSfx;
    private new AudioSource audio;
    public MeshRenderer muzzleFlash;
    private bool fireBullet = false;

    private RaycastHit hit;

    float time;
    float timeDelay;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;

        time = 0f;
        timeDelay = .5f;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            fireBullet = true;                         
        }

        if (Input.GetMouseButtonUp(0))
        {
            fireBullet = false;
        }


        time = time + 1f * Time.deltaTime;

        if (time >= timeDelay && fireBullet)
        {
            time = 0f;
            FIre();

            // Raycast || RaycastAll => GarbageCollector
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, 10.0f, 1<<8))
            {
                //Debug.Log($"hit={hit.collider.name}");
                hit.collider.GetComponent<MonsterCtrl>().OnDamage(hit.point, hit.normal, 25.0f);
            }
        }            
    }

    // Co-routine vs Thread
    IEnumerator showMuzzleFlash()
    {
        // Texture Offset
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0,2)) * 0.5f;
        muzzleFlash.material.mainTextureOffset = offset;

        float scale = Random.Range(1.0f, 2.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.1f); // sub-routine execution stop

        muzzleFlash.enabled = false;
    }

    void FIre()
    {
        // 동적으로 생성 (프리팹, 위치, 각도)
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        // audio.play 이전 재생 파일 중지 및 재생
        audio.PlayOneShot(fireSfx, 0.8f); // 동시 재생
        StartCoroutine(showMuzzleFlash());
    }

    // xyzw Quaternion (사원수 4차원) 복소수 4차원 벡터
}
