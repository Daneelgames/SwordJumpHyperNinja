using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherKissController : MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem particles;

    void FixedUpdate()
    {
        Vector2 dir = GameManager.instance.pc.transform.position - transform.position;
        rb.velocity = dir.normalized * 6;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            Destroy(gameObject, 5);
        }
    }
}