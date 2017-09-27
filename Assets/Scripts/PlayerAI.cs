using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{

    public List<P2AIControlPointController> controlPoints = new List<P2AIControlPointController>();
    public List<P2AIControlPointController> tempList = new List<P2AIControlPointController>();
    public List<P2AIControlPointController> path = new List<P2AIControlPointController>();

    public P2AIControlPointController targetControlPoint;
    public PlayerController pc2;
    Transform enemy;

    public bool enemyInRange = false;

    RaycastHit2D directionRaycast;
    RaycastHit2D upRaycast;
    RaycastHit2D upRightRaycast;
    RaycastHit2D rightRaycast;
    RaycastHit2D downRightRaycast;
    RaycastHit2D downRaycast;
    RaycastHit2D downLeftRaycast;
    RaycastHit2D leftRaycast;
    RaycastHit2D upLeftRaycast;

    public void AddCotrolPoint(P2AIControlPointController point)
    {
        controlPoints.Add(point);
    }

    void Start()
    {
        PickControlPoint();
        InvokeRepeating("RaycastSolids", 0.01f, 0.01f);
    }

    void Update()
    {
        GetInput();
        //TargetSpear();
    }
    public void SetEnemyInRange(bool inRange, Transform _enemy)
    {
        enemyInRange = inRange;
        enemy = _enemy;
    }
    void RaycastSolids()
    {
        directionRaycast = Physics2D.Raycast(transform.position, pc2.rb.velocity.normalized, 1, pc2.solidMask);

        upRaycast = Physics2D.Raycast(transform.position, Vector2.up, 1.25f, pc2.solidMask);
        upRightRaycast = Physics2D.Raycast(transform.position, new Vector2(1, 1), 1, pc2.solidMask);
        rightRaycast = Physics2D.Raycast(transform.position, Vector2.right, 1.25f, pc2.solidMask);
        downRightRaycast = Physics2D.Raycast(transform.position, new Vector2(1, -1), 1, pc2.solidMask);
        downRaycast = Physics2D.Raycast(transform.position, Vector2.down, 1.25f, pc2.solidMask);
        downLeftRaycast = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 1, pc2.solidMask);
        leftRaycast = Physics2D.Raycast(transform.position, Vector2.left, 1.25f, pc2.solidMask);
        upLeftRaycast = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 1, pc2.solidMask);
        TargetSpear();
    }

    void TargetSpear()
    {
        Vector2 dir = Vector2.zero;

        float verDistance = Mathf.Abs(transform.position.y - targetControlPoint.transform.position.y);
        float horDistance = Mathf.Abs(transform.position.x - targetControlPoint.transform.position.x);

        if (targetControlPoint.transform.position.y > transform.position.y + 0.2f && verDistance > horDistance)
            dir = new Vector2(dir.x, -1);
        if (targetControlPoint.transform.position.x > transform.position.x + 0.2f && horDistance > verDistance && targetControlPoint.transform.position.y >= transform.position.y - 0.2f)
        {
            dir = new Vector2(-1, dir.y);
            if (!downRightRaycast && downLeftRaycast)
                dir = new Vector2(-1, -1); // if player is on right and ai should jump to him
        }
        if (targetControlPoint.transform.position.x < transform.position.x - 0.2f && horDistance > verDistance && targetControlPoint.transform.position.y >= transform.position.y - 0.2f)
        {
            dir = new Vector2(1, dir.y);
            if (downRightRaycast && !downLeftRaycast)
                dir = new Vector2(1, -1); // if player is on left and ai should jump to him
        }

        if (targetControlPoint.transform.position.y > transform.position.y + 0.2f)
        {
            if (upLeftRaycast)
                dir = new Vector2(-1, -1);
            if (upRightRaycast)
                dir = new Vector2(1, -1);
            if (leftRaycast)
                dir = new Vector2(-1, -1);
            if (rightRaycast)
                dir = new Vector2(1, -1);
        }
        else if (targetControlPoint.transform.position.y < transform.position.y + 0.2f)
        {
            if (upLeftRaycast)
                dir = new Vector2(-1, dir.y);
            if (upRightRaycast)
                dir = new Vector2(1, dir.y);
            if (leftRaycast)
                dir = new Vector2(-1, dir.y);
            if (rightRaycast)
                dir = new Vector2(1, dir.y);

        }

        if (enemyInRange)
        {
            dir = enemy.transform.position - transform.position;
            dir = dir.normalized;
        }

        if (upLeftRaycast && upLeftRaycast.collider.gameObject.layer == 16 && Vector2.Distance(upLeftRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(-1, 1);
        if (upRightRaycast && upRightRaycast.collider.gameObject.layer == 16 && Vector2.Distance(upRightRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(-1, 1);
        if (downLeftRaycast && downLeftRaycast.collider.gameObject.layer == 16 && Vector2.Distance(downLeftRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(-1, -1);
        if (downRightRaycast && downRightRaycast.collider.gameObject.layer == 16 && Vector2.Distance(downRightRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(-1, 1);
        if (leftRaycast && leftRaycast.collider.gameObject.layer == 16 && Vector2.Distance(leftRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(-1, 0);
        if (rightRaycast && rightRaycast.collider.gameObject.layer == 16 && Vector2.Distance(rightRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(1, 0);
        if (downRaycast && downRaycast.collider.gameObject.layer == 16 && Vector2.Distance(downRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(0, -1);
        if (upRaycast && upRaycast.collider.gameObject.layer == 16 && Vector2.Distance(upRaycast.point, transform.position) < 0.75f)
            dir = new Vector2(0, 1);

        //if (directionRaycast)
        //    dir = pc2.rb.velocity.normalized;

        pc2.spearController.GetAiInput(dir);
    }

    public void PickControlPoint()
    {
        // GET CLOSEST TO PLAYER 
        P2AIControlPointController closestPoint = controlPoints[Random.Range(0, controlPoints.Count)];
        float distance = 1000;
        foreach (P2AIControlPointController i in controlPoints)
        {
            float newDistance = Vector3.Distance(GameManager.instance.pc.transform.position, i.transform.position);
            if (newDistance < distance)
            {
                closestPoint = i;
                distance = newDistance;
            }
        }
        targetControlPoint = closestPoint;
        SetPath();

        /*
            // GET RANDOM
            tempList = new List<P2AIControlPointController>(controlPoints);

            P2AIControlPointController p2AiPointPosition = GetClosestPoint(); //point of p2ai position

            P2AIControlPointController closestPoint = tempList[Random.Range(0, tempList.Count)]; // other point closest to p2ai
            float distance = 1000;
            for (int i = tempList.Count - 1; i >= 0; i--) // find closest point
            {
                float newDistance = Vector3.Distance(transform.position, tempList[i].transform.position);
                if (newDistance < distance)
                {
                    closestPoint = tempList[i];
                    distance = newDistance;
                }
            }
            tempList.Remove(closestPoint);

            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                foreach (P2AIControlPointController j in tempList)
                {
                    if (j == p2AiPointPosition)
                    {
                        tempList.RemoveAt(i);
                    }
                }
            }

            targetControlPoint = tempList[Random.Range(0, tempList.Count)];
        */
    }

    P2AIControlPointController GetClosestPoint()
    {
        P2AIControlPointController closestPoint = controlPoints[Random.Range(0, controlPoints.Count)];
        float distance = 1000;
        for (int i = controlPoints.Count - 1; i >= 0; i--) // find closest point
        {
            float newDistance = Vector3.Distance(transform.position, controlPoints[i].transform.position);
            if (newDistance < distance)
            {
                closestPoint = controlPoints[i];
                distance = newDistance;
            }
        }
        return closestPoint;
    }

    void SetPath()
    {
        path.Clear();
        path.Add(targetControlPoint); // add target point in path

        for (int j = 0; j < 100; j++)
        {
            P2AIControlPointController closestPoint = path[path.Count - 1].connectedPoints[Random.Range(0, path[path.Count - 1].connectedPoints.Count)]; // find closest
            float distance = 1000;
            for (int i = path[path.Count - 1].connectedPoints.Count - 1; i >= 0; i--)
            {
                float newDistance = Vector3.Distance(transform.position, path[path.Count - 1].connectedPoints[i].transform.position);
                if (newDistance < distance)
                {
                    closestPoint = path[path.Count - 1].connectedPoints[i];
                    distance = newDistance;
                }
            }
            if (closestPoint == GetClosestPoint())
                break;
            path.Add(closestPoint);
        }
        targetControlPoint = path[path.Count - 1];
    }

    public void PointPassed(P2AIControlPointController point)
    {
        /*
            pointInPathPassed += 1;
            if (targetControlPoint != path[0])
                targetControlPoint = path[path.Count - 1 - pointInPathPassed];
            else if (targetControlPoint == path[0])
                PickControlPoint();
        */

        if (path.Count > 1)
        {
            path.RemoveAt(path.Count - 1);
            targetControlPoint = path[path.Count - 1];
        }
        else
        {
            PickControlPoint();
        }
    }

    void GetInput()
    {
        // HORIZONTAL
        float hSpeed = 0;

        if (targetControlPoint.transform.position.x < transform.position.x - 0.2f)
            hSpeed = -1f;
        else if (targetControlPoint.transform.position.x > transform.position.x + 0.2f)
            hSpeed = 1;

        pc2.P2AIhSpeed(hSpeed);
    }
}