using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject bloodSplatter;
    public Animator anim;
    public BloodHolderController bloodHolder;

    void Start()
    {
        Time.timeScale = 1;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spear")
        {
            StartCoroutine("FinishLevel");
        }
    }

    IEnumerator FinishLevel()
    {
        bloodHolder.EmitBlood();
        anim.SetTrigger("Dead");
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        GameManager.instance.FinishLevel();
    }
}