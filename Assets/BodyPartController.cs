using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float force = 10;
    void Start()
    {
        Vector2 randomForce = new Vector2(Random.value, Random.value).normalized;
        rb.AddForceAtPosition(randomForce * force, GameManager.instance.pc.transform.position, ForceMode2D.Impulse);
        //rb.AddTorque(Random.Range(-10, 10) * force);
        rb.angularVelocity = Random.value * force;
        DontDestroyOnLoad(gameObject);
    }
}
