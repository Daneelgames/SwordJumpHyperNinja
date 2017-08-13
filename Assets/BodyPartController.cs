using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{

    public Rigidbody2D rb;
    void Start()
    {
        Vector2 randomForce = new Vector2(Random.value, Random.value).normalized;
        rb.AddForceAtPosition(randomForce * 10f, GameManager.instance.pc.transform.position, ForceMode2D.Impulse);
		rb.AddTorque(Random.Range(-90, 90));
        DontDestroyOnLoad(gameObject);
    }
}
