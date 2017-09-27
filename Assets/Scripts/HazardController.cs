using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    public float dangerDelay = 0;
    public Collider2D coll;
    bool dangerous = true;

    void Start()
    {
        if (dangerDelay > 0)
        {
            dangerous = false;
        }
    }

    void Update()
    {
        if (dangerDelay > 0)
        {
            dangerDelay -= Time.deltaTime;
        }
        else
        {
            dangerous = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 15)
        {
            if (dangerous && !GameManager.instance.pc.dead)
            {
                if (other.gameObject.tag == "PlayerHealth")
                {
                    if (!GameManager.instance.pc2)
                    {
                        dangerous = false;
                        KillPlayer();
                    }
                    else if (!GameManager.instance.pc2.dead)
                    {
                        dangerous = false;
                        KillPlayer();
                    }
                }
                else if (other.gameObject.tag == "P2Health" && GameManager.instance.pc2 && !GameManager.instance.pc2.dead)
                {
                    dangerous = false;
                    KillP2();
                }
            }
        }
    }

    void KillPlayer()
    {
        GameManager.instance.RestartLevel(false, false);
        if (coll)
            coll.enabled = false;
    }
    void KillP2()
    {
        GameManager.instance.RestartLevel(true, false);
        if (coll)
            coll.enabled = false;
    }
}