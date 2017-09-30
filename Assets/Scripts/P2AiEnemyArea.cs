using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2AiEnemyArea : MonoBehaviour
{
    public PlayerAI p2ai;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 9)
        {
            if (coll.gameObject == GameManager.instance.pc.gameObject)
            {
                p2ai.SetEnemyInRange(true, coll.gameObject.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 9)
        {
            if (coll.gameObject == GameManager.instance.pc.gameObject)
            {
                p2ai.SetEnemyInRange(false, coll.gameObject.transform);
            }
        }
    }
}