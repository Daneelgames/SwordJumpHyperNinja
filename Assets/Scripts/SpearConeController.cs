using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearConeController : MonoBehaviour
{

    public Animator anim;
    public PlayerController pc;
    public float maxCooldown = 0.1f;
    float cooldown = 0;

    void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Bounce(other);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        Bounce(other);
    }
    void Bounce(Collision2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        {
            if (cooldown <= 0)
            {
                cooldown = maxCooldown;
                Vector3 bouncePos = pc.spearController.spearTarget.transform.position;
                pc.Bounce(bouncePos);
                //anim.SetTrigger("Reset");
            }
        }
    }
}