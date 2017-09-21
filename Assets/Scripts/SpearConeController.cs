using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearConeController : MonoBehaviour
{

    public Animator anim;
    public PlayerController pc;
    public float maxCooldown = 0.1f;
    float cooldown = 0;
    public AudioClip swordWooshSfx;
    public AudioClip swordHitSfx;
    float cooldownSfx = 0f;
    //bool canStay = true;

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldownSfx > 0)
        {
            cooldownSfx -= Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Bounce(other.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 13)
            Bounce(other.gameObject);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        //if (canStay && cooldown <= 0)
        if (cooldown <= 0)
        {
            Bounce(other.gameObject);
            //canStay = false;
        }
    }
    /*
    void OnCollisionExit2D(Collision2D other)
    {
        if (canStay)
        {
            canStay = true;
        }
    }
     */

    void PlaySound()
    {
        if (cooldownSfx <= 0)
        {
            cooldownSfx = 0.1f;
            AudioSource au = gameObject.AddComponent<AudioSource>();
            au.clip = swordWooshSfx;
            au.volume = Random.Range(0.1f, 0.25f);
            au.pitch = Random.Range(0.8f, 1.2f);
            au.Play();
            Destroy(au,0.5f);
            AudioSource au2 = gameObject.AddComponent<AudioSource>();
            au2.clip = swordHitSfx;
            au2.volume = Random.Range(0.1f, 0.25f);
            au2.pitch = Random.Range(0.8f, 1.2f);
            au2.Play();
            Destroy(au2,0.5f);

            //swordSource.pitch = Random.Range(0.75f, 1.25f);
            //swordSource.Play();
        }
    }


    void Bounce(GameObject other)
    {
        //print("bounce");
        if (other.layer == 8 || other.layer == 11 || other.layer == 16)
        {
            PlaySound();

            cooldown = maxCooldown;
            Vector3 bouncePos = pc.spearController.spearTarget.transform.position;
            pc.Bounce(bouncePos);
        }
    }
}