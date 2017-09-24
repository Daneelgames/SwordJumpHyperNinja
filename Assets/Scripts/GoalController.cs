using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject bloodSplatter;
    public Animator anim;
    public BloodHolderController bloodHolder;
    int direction;
    public AudioSource au;
    public string menu = "";

    void Start()
    {
        direction = Mathf.RoundToInt(anim.gameObject.transform.parent.localScale.x);
        Time.timeScale = 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spear")
        {
            StartCoroutine("FinishLevel");
        }
    }

    IEnumerator FinishLevel()
    {
        //GameManager.instance.activeCam.GoalZoom();
        if (GameManager.instance.activeCam)
            GameManager.instance.activeCam.SetTrigger("ShakeBig");


		GameManager.instance.levelClear = true;
        au.Play();

        if (bloodHolder)
            bloodHolder.EmitBlood();

        anim.SetTrigger("Dead");

        if (bloodSplatter)
            Instantiate(bloodSplatter, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            
        GameManager.instance.pc.JumpSoundStop();
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5f);
        GameManager.instance.Fade(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        GameManager.instance.FinishLevel(menu);
    }

    public void Alarm()
    {
        anim.SetBool("Alarm", true);
    }
}