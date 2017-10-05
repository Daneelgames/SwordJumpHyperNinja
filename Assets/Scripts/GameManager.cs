using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

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
    public AudioMixerGroup auMixer;
    public Camera renderCam;
    public Animator camAnim;
    public PostProcessingBehaviour postProcessing;
    public CameraResetManager camReset;
    public MusicController musicController;

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
        if (Input.GetKeyDown(KeyCode.F2))
            FinishLevel("");

        if (Input.GetKeyDown(KeyCode.F1) && SceneManager.GetActiveScene().buildIndex != 0)
            PastLevel();
    }

    public IEnumerator FinishLevel(string menu)
    {
        //GameManager.instance.activeCam.GoalZoom();
        if (GameManager.instance.activeCam)
            GameManager.instance.activeCam.SetTrigger("ShakeBig");

        if (pc.dead && pc2 && pc2.dead)
        {
            StartCoroutine(PlayerDead(false, false));
            yield break;
        }

        GameManager.instance.levelClear = true;
        GameManager.instance.pc.JumpSoundStop();
        if (pc2)
            GameManager.instance.pc2.JumpSoundStop();
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5f);
        GameManager.instance.Fade(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;

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

        Fade(false);
        levelClear = false;
        SetAudioPitch(false);

        // RETURN CAMERA TO GAMEMANAGER
        renderCam.transform.SetParent(transform);
        renderCam.transform.localPosition = Vector3.zero;
        camReset.Reset();

        if (menu == "")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (menu == "Play")
            SceneManager.LoadScene(4); // LOAD GAME HERE
        else if (menu == "Arena")
            SceneManager.LoadScene(2);
        else if (menu == "Options")
            SceneManager.LoadScene(1);
        else if (menu == "BackToMenu")
            SceneManager.LoadScene(0);
        else if (menu == "ExitGame")
            Application.Quit();
        else if (menu == "Fullscreen")
        {
            Screen.fullScreen = !Screen.fullScreen;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else // if player choses level from world map
        {
            ChooseLevel(menu);
        }
    }

    void ChooseLevel(string level)
    {
        SceneManager.LoadScene(level);
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

        SetAudioPitch(false);

        // RETURN CAMERA TO GAMEMANAGER
        renderCam.transform.SetParent(transform);
        renderCam.transform.localPosition = Vector3.zero;
        camReset.Reset();

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

    public void RestartLevel(bool p2, bool deadBySpear)
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
        StartCoroutine(PlayerDead(p2, deadBySpear));
    }

    public void Fade(bool black)
    {
        fadeAnim.SetBool("Black", black);
    }

    IEnumerator PlayerDead(bool p2, bool deadBySpear) // if P2 is dead
    {
        pc.ToggleSlowMo(false);
        levelClear = true;
        if (!p2) // if Player is dead
        {
            if (acc)
                acc.AddPoint("P2");
            pc.Dead();
        }
        if (p2)// if p2 is dead 
        {
            if (acc)
                acc.AddPoint("Player");
            pc2.Dead();
        }

        if (pc2 && pc2.ai && pc2.dead && !pc.dead && deadBySpear)
        {
            StartCoroutine("FinishLevel", "");
        }
        else
        {
            Fade(true);
            yield return new WaitForSecondsRealtime(0.5f);
            Fade(false);
            levelClear = false;
            SetAudioPitch(false);
            pc.ToggleSlowMo(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SetPlayer(PlayerController _pc)
    {
        pc = _pc;
    }
    public void SetP2(PlayerController _pc)
    {
        pc2 = _pc;
    }
    public Animator SetActiveCamera(CameraMovement cam)
    {
        activeCam = cam;
        renderCam.transform.SetParent(activeCam.transform);
        renderCam.transform.localPosition = Vector3.zero;
        return camAnim;
    }

    public void SetAudioPitch(bool slowMo)
    {
        if (slowMo)
            auMixer.audioMixer.SetFloat("gamePitch", 0.25f);
        else
            auMixer.audioMixer.SetFloat("gamePitch", 1);
    }
}