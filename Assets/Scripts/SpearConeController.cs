using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearConeController : MonoBehaviour
{

    public Animator anim;
    public PlayerController pc;
    public float maxCooldown = 0.1f;
    float cooldown = 0;
    bool canStay = true;

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Bounce(other.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Bounce(other.gameObject);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (canStay && cooldown <= 0)
        {
            Bounce(other.gameObject);
            canStay = false;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (canStay)
        {
            canStay = true;
        }
    }
    void Bounce(GameObject other)
    {
        //print("bounce");
        if (other.layer == 8 || other.layer == 11 || other.layer == 16)
        {
            cooldown = maxCooldown;
            Vector3 bouncePos = pc.spearController.spearTarget.transform.position;
            pc.Bounce(bouncePos);
        }
    }
}