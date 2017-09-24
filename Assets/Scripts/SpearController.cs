using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    public PlayerController pc;
    public Rigidbody2D rb;
    Vector2 spearDirection = new Vector2(1, 0);
    public float turnSpeed = 4;
    public GameObject spearTarget;
    public Collider2D spearCone;

    void Start()
    {
        TogglePogo(false);
        transform.SetParent(null);
    }

    void Update()
    {
        GetInput();

    }
    void FixedUpdate()
    {
        ApplyForces();
        rb.MovePosition(pc.gameObject.transform.position);
    }
    void GetInput()
    {
        // SPEAR TARGETING
        
        float hs;
        float vs;

        if (!pc.p2)
        {
            hs = Input.GetAxis("HorizontalSpear");
            vs = Input.GetAxis("VerticalSpear");
        }
        else
        {
            hs = Input.GetAxis("HorizontalSpearP2");
            vs = Input.GetAxis("VerticalSpearP2");
        }

        //if (Input.GetAxisRaw("HorizontalSpear") != 0 || Input.GetAxisRaw("VerticalSpear") != 0)
        spearDirection = new Vector2(hs, vs);

        if (pc)
            PositionSpearTarget();
    }
    void PositionSpearTarget()
    {
        //spearTarget.transform.position = Vector2.Lerp(spearTarget.transform.position, new Vector2(transform.position.x + spearDirection.x,transform.position.y + spearDirection.y), 1);
        Vector2 normalizedOffset = spearDirection.normalized;
        Vector2 newPos = new Vector2(pc.transform.position.x + normalizedOffset.x, pc.transform.position.y + normalizedOffset.y);
        //spearTarget.transform.position = Vector2.Lerp(spearTarget.transform.position, newPos, 0.1f);
        spearTarget.transform.position = newPos;
        // NEED TO MOVE TARGET AROUND CIRCLE
    }
    void ApplyForces()
    {
        Vector2 directionSpearTarget = spearTarget.transform.position - transform.position;
        Vector2 directionSpearCone = spearCone.transform.position - transform.position;

        var angle = Mathf.Atan2(directionSpearTarget.y, directionSpearTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //rb.MoveRotation(angle);

        //Vector2 newSpearDir = new Vector2(Input.GetAxisRaw("HorizontalSpear"), Input.GetAxisRaw("VerticalSpear"));
        if (spearDirection == Vector2.zero)
        {
            TogglePogo(false);
        }
        else
            TogglePogo(true);
    }
    void TogglePogo(bool active)
    {
        spearCone.enabled = active;
    }
}