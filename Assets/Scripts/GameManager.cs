using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator fadeAnim;
    public static GameManager instance;
    public PlayerController pc;
    public PlayerController pc2;
    public CameraMovement activeCam;
    public List<GameObject> bloodSplatters = new List<GameObject>();
    public List<GameObject> bodyParts = new List<GameObject>();
	public bool levelClear = false;
    public ArenaCanvasController acc;

    public void SetArenaCanvasController(ArenaCanvasController _acc)
    {
        acc = _acc;
    }

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
            FinishLevel("");

        if (Input.GetKeyDown(KeyCode.F1) && SceneManager.GetActiveScene().buildIndex != 0)

            PastLevel();
    }

    public void FinishLevel(string menu)
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
        if (menu == "")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (menu == "Play")
            SceneManager.LoadScene(3); // LOAD GAME HERE
        else if(menu == "Arena")
            SceneManager.LoadScene(2);
        else if (menu == "Options")
            SceneManager.LoadScene(1);
        else if (menu == "BackToMenu")
            SceneManager.LoadScene(0);
        else if (menu == "ExitGame")
            Application.Quit();

		levelClear = false;
        if (acc)
            Destroy(acc.gameObject);
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

        if (acc)
            Destroy(acc.gameObject);

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

    public void RestartLevel(bool p2)
    {
        GameObject blood;
        GameObject blood2;
        if (p2 == false)
        {
            blood = Instantiate(pc.bloodSplatter, pc.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
            blood2 = Instantiate(pc.bloodParticles, pc.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        }
        else
        {
            blood = Instantiate(pc2.bloodSplatter, pc2.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
            blood2 = Instantiate(pc2.bloodParticles, pc2.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject; 
        }
        DontDestroyOnLoad(blood);
        DontDestroyOnLoad(blood2);
        bloodSplatters.Add(blood);
		levelClear = true;
        StartCoroutine(PlayerDead(p2));
    }

    public void Fade(bool black)
    {
        fadeAnim.SetBool("Black", black);
    }

    IEnumerator PlayerDead(bool p2)
    {
        if (!p2)
        {
            if (acc)
                acc.AddPoint("P2");
            pc.Dead();
        }
        else
        {
            if (acc)
                acc.AddPoint("Player");
            pc2.Dead();
        }
        Fade(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Fade(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		levelClear = false;
    }

    public void SetPlayer(PlayerController _pc)
    {
        pc = _pc;
    }
    public void SetP2(PlayerController _pc)
    {
        pc2 = _pc;
    }
    public void SetActiveCamera(CameraMovement cam)
    {
        activeCam = cam;
    }

}