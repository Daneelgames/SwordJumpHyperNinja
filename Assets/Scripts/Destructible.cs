using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int health = 1;
    public float maxHurtCooldown = 0.3f;
    float hurtCooldown = 0f;
    public LinearBoundsMovement _LBM;
    public GameObject deathParticles;
    public GameObject slashParticles;
    public List<GameObject> bodyParts = new List<GameObject>();
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spear" && other.gameObject.layer == 10)
        {
            print("mob hit");
            if (hurtCooldown <= 0)
            {
                hurtCooldown = maxHurtCooldown;
                health -= 1;

                if (slashParticles != null)
                {
                    GameObject blood = Instantiate(slashParticles, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
                    DontDestroyOnLoad(blood);
                }
                if (health == 0)
                {
                    Death();
                }
            }
        }
    }

    void Update()
    {
        if (hurtCooldown > 0)
        {
            hurtCooldown -= Time.deltaTime;
        }
    }

    void Death()
    {

        if (deathParticles != null)
        {
            GameObject blood = Instantiate(deathParticles, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
            DontDestroyOnLoad(blood);
            GameManager.instance.bloodSplatters.Add(blood);
        }
        _LBM.DestroyBounds();
        GameManager.instance.activeCam.SetTrigger("Shake");

        foreach (GameObject c in bodyParts)
        {
            GameObject part = Instantiate(c, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
        Destroy(gameObject);
    }
}