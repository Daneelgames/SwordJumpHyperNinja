using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBoundsMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 1;
    public Collider2D coll;
    public MovementBoundController boundLeft;
    public MovementBoundController boundRight;
    MovementBoundController target;
    Vector2 newVel;

    void Start()
    {
        boundLeft.SetMaster(this);
        boundRight.SetMaster(this);
        target = boundRight;
        SetDirection();
    }

    void SetDirection()
    {
        newVel = target.transform.position - transform.position;
        newVel.Normalize();
    }
    void FixedUpdate()
    {
        //Vector2 newVel = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = newVel * speed;
    }

    public void ChangeDirection()
    {
        if (target == boundLeft)
            target = boundRight;
        else
            target = boundLeft;
            
        SetDirection();
    }
    public void DestroyBounds()
    {
        Destroy(boundLeft.gameObject);
        Destroy(boundRight.gameObject);
    }
}