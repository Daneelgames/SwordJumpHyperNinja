using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController pc;
    public CameraMovement activeCam;
    List<GameObject> bloodSplatters = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void FinishLevel()
    {
        //temp
        foreach (GameObject c in bloodSplatters)
        {
            Destroy(c);
        }
        bloodSplatters.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    IEnumerator PlayerDead()
    {
        pc.Dead();
        yield return new WaitForSecondsRealtime(0.25f);
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