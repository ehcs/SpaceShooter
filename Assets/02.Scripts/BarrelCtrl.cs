using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelCtrl : MonoBehaviour
{    
    int hitCount; // 자동으로 0 setting?
    public Texture[] textures;
    public new MeshRenderer renderer;
    public GameObject expEffect;
    public AudioClip expSfx;
    private new AudioSource audio;

    /*
     * Random.Range(min, max);
     * Range(int, int) max exclusive
     * Range(float, float) max inclusive
     */
    // Start is called before the first frame update
    void Start()
    {
        int idx = Random.Range(0, textures.Length);
        renderer = GetComponentInChildren<MeshRenderer>();
        renderer.material.mainTexture = textures[idx];

        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount == 3)
            {
                ExpBarrel();

                audio.PlayOneShot(expSfx, 1.0f);
            }
        }
    }

    void ExpBarrel()
    {
        var rb = this.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1500.0f);
        Destroy(this.gameObject, 2.0f);

        // 폭발효과
        GameObject exp = Instantiate(expEffect, transform.position, Quaternion.identity); // identity 원본그대로
        Destroy(exp, 10.0f);
    }
        
}
