using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBoundsMovement : MonoBehaviour
{
    int direction = 1;
    public Rigidbody2D rb;
    public float speed = 1;
    public Collider2D coll;
    public MovementBoundController boundLeft;
    public MovementBoundController boundRight;

    void Start()
    {
        boundLeft.SetMaster(this);
        boundRight.SetMaster(this);
    }

    void FixedUpdate()
    {
        Vector2 newVel = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = newVel;
    }

    public void ChangeDirection()
    {
        switch (direction)
        {
            case -1:
                direction = 1;
                break;

            default:
                direction = -1;
                break;
        }
    }
    public void DestroyBounds()
    {
        Destroy(boundLeft.gameObject);
        Destroy(boundRight.gameObject);
    }
}