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

        if (menu == "")
        GameManager.instance.goal = gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spear")
        {
            FinishLevel();
        }
    }

    void FinishLevel()
    {
        au.Play();

        if (bloodHolder)
            bloodHolder.EmitBlood();

        anim.SetTrigger("Dead");

        if (bloodSplatter)
            Instantiate(bloodSplatter, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        GameManager.instance.StartCoroutine("FinishLevel", menu);
    }

    public void Alarm()
    {
        anim.SetBool("Alarm", true);
    }
}