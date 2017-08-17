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
    int direction = 1;

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

        StartCoroutine("Squash");
        SetDirection();
    }

    IEnumerator Squash()
    {
        float t = 0f;
        float duration = 0.1f;
        while (duration > 0f)
        {
            print("squash");
            duration -= Time.deltaTime;
            t += Time.deltaTime / 0.1f;
            float newScaleX = Mathf.Lerp(transform.localScale.x, 0.5f * direction, t);
            transform.localScale = new Vector2(newScaleX, transform.localScale.y);
            yield return null;
        }
        duration = 0.1f;
        direction *= -1;
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            t += Time.deltaTime / 0.1f;
            float newScaleX = Mathf.Lerp(transform.localScale.x, direction, t);
            transform.localScale = new Vector2(newScaleX, transform.localScale.y);
            yield return null;
        }
    }

    public void DestroyBounds()
    {
        Destroy(boundLeft.gameObject);
        Destroy(boundRight.gameObject);
    }
}