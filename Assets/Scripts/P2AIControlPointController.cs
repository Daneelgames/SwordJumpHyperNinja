using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2AIControlPointController : MonoBehaviour
{

    public List<P2AIControlPointController> connectedPoints = new List<P2AIControlPointController>();
    PlayerAI pc2ai;
    void Start()
    {
        pc2ai = GameManager.instance.pc2.ai;
        pc2ai.AddCotrolPoint(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (P2AIControlPointController i in connectedPoints)
        {
            Gizmos.DrawLine(transform.position, i.transform.position);

        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject == pc2ai.gameObject)
        {
            pc2ai.PointPassed(this);
        }
        else if (coll.gameObject == GameManager.instance.pc.gameObject)
        {
            pc2ai.PickControlPoint();
        }
    }
}