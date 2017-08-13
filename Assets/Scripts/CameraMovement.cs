using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Animator anim;
    Vector2 velocity;
    public float smoothTimeX;
    public float smoothTimeY;
    GameObject player;

    public bool bounds = true;
    public Vector2 minPos;
    public Vector2 maxPos;
    bool goalZoom = false;
    void Awake()
    {
        if (GameManager.instance.activeCam == null)
        {
            GameManager.instance.SetActiveCamera(this);
            //player = GameObject.FindWithTag("Player");
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetPlayer(PlayerController pc)
    {
        player = pc.gameObject;
    }

    void LateUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds && !goalZoom)
        {
            float newX = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
            float newY = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    public void GoalZoom()
    {
        float t = 0;
        Vector2 startPosition = transform.position;
        float timeToReachTarget = 3;
        anim.SetBool("Zoom", true);

        goalZoom = true;

        while (t < 3)
        {
            t += Time.unscaledDeltaTime / timeToReachTarget;
            transform.position = Vector2.Lerp(startPosition, new Vector3(GameManager.instance.pc.transform.position.x, GameManager.instance.pc.transform.position.y, -10f), t);
        }
    }
}