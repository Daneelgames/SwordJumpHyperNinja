using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.layer == 9)
        {
            if (!GameManager.instance.pc.dead)
                GameManager.instance.RestartLevel();
        }
    }
}