using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicController : MonoBehaviour
{

    public bool endGame = false;

    void Start()
    {
        if (endGame)
            ChangeMusic(-1);
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            ChangeMusic(2);
    }

    void ChangeMusic(int index)
    {
        if (index == -1)
        {
            GameManager.instance.musicController.au.Stop();
        }
        else if (GameManager.instance.musicController.au.clip != GameManager.instance.musicController.music[index])
        {
            GameManager.instance.musicController.au.Stop();
            GameManager.instance.musicController.au.clip = GameManager.instance.musicController.music[index];
            GameManager.instance.musicController.au.Play();
        }
    }
}
