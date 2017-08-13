using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject bloodSplatter;
    public Animator anim;
    public BloodHolderController bloodHolder;
    int direction;

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
        bloodHolder.EmitBlood();
        anim.SetTrigger("Dead");
        Instantiate(bloodSplatter, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        GameManager.instance.FinishLevel();
    }

    public void Alarm()
    {
        anim.SetBool("Alarm", true);
    }
}