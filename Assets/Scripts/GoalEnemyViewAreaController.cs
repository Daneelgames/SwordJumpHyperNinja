using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEnemyViewAreaController : MonoBehaviour
{
    public GoalController goal;
    public LayerMask mask;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("SearchPlayer", 0.1f, 0.1f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CancelInvoke();
        }
    }

    void SearchPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GameManager.instance.pc.transform.position - transform.position, Vector2.Distance(transform.position, GameManager.instance.pc.transform.position), mask);
        Debug.DrawLine(transform.position, GameManager.instance.pc.transform.position, Color.green);
        if (hit)
        {
//            print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Player")
            {
                goal.Alarm();
                CancelInvoke();
            }
        }
    }
}
