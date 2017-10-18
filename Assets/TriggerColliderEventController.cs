using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderEventController : MonoBehaviour
{
    public string eventName;
    public Animator eventAnim;
    public AudioClip eventSound;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && eventName == "KillMother")
        {
            GameObject sfx = Instantiate(GameManager.instance.pc.swordSlashParticles, new Vector2(-7.5f, 3.5f), Quaternion.Euler(0, 0, 270)) as GameObject;
            sfx.transform.localScale *= 2;
            Instantiate(GameManager.instance.pc.bloodParticles, new Vector2(-7.5f, 3.5f), Quaternion.identity);
            Instantiate(GameManager.instance.pc.bloodSplatter, new Vector2(-7.5f, 3.5f), Quaternion.identity);
            GameManager.instance.ToggleGoal(true);
            eventAnim.SetTrigger("Dead");

            GameObject auObj = new GameObject();
            AudioSource auNew = auObj.AddComponent<AudioSource>();
            auNew.clip = eventSound;
			auNew.outputAudioMixerGroup = GameManager.instance.auMixer;
            auNew.Play();
            Destroy(auObj, 1f);

            Destroy(gameObject);
        }
    }
}