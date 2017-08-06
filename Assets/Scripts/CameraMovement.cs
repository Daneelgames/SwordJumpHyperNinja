using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector2 velocity;
    public float smoothTimeX;
    public float smoothTimeY;
    GameObject player;

    public bool bounds = true;
    public Vector2 minPos;
    public Vector2 maxPos;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void LateUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {
            float newX = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
            float newY = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }
}
