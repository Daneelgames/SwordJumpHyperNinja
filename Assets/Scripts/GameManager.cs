﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator fadeAnim;
    public static GameManager instance;
    public PlayerController pc;
    public CameraMovement activeCam;
    public List<GameObject> bloodSplatters = new List<GameObject>();
    public List<GameObject> bodyParts = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Fade(false);
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) && SceneManager.GetActiveScene().buildIndex + 1 != null)
            FinishLevel();

        if (Input.GetKeyDown(KeyCode.F1) && SceneManager.GetActiveScene().buildIndex != 0)

            PastLevel();
    }

    public void FinishLevel()
    {
        foreach (GameObject c in bloodSplatters)
        {
            Destroy(c);
        }
        bloodSplatters.Clear();

        foreach (GameObject c in bodyParts)
        {
            Destroy(c);
        }
        bodyParts.Clear();

        Destroy(activeCam.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Fade(false);
    }

    public void PastLevel()
    {
        foreach (GameObject c in bloodSplatters)
        {
            Destroy(c);
        }
        bloodSplatters.Clear();

        foreach (GameObject c in bodyParts)
        {
            Destroy(c);
        }
        bodyParts.Clear();

        Destroy(activeCam.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void AddBodyParts(GameObject go)
    {
        bodyParts.Add(go);
    }

    public void RemoveBodyPart(GameObject go)
    {
        bodyParts.Remove(go);
        //bodyParts.Sort();
    }

    public void RestartLevel()
    {
        GameObject blood = Instantiate(pc.bloodSplatter, pc.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        GameObject blood2 = Instantiate(pc.bloodParticles, pc.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        DontDestroyOnLoad(blood);
        DontDestroyOnLoad(blood2);
        bloodSplatters.Add(blood);
        StartCoroutine("PlayerDead");
    }

    public void Fade(bool black)
    {
        fadeAnim.SetBool("Black", black);
    }

    IEnumerator PlayerDead()
    {
        pc.Dead();
        Fade(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Fade(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPlayer(PlayerController _pc)
    {
        pc = _pc;
    }
    public void SetActiveCamera(CameraMovement cam)
    {
        activeCam = cam;
    }

}