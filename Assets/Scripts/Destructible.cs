using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{

    public HorizontalBoundsMovement _HBM;
    public GameObject deathParticles;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spear" && other.gameObject.layer == 10)
        {
            if (deathParticles != null)
            {
                Instantiate(deathParticles, transform.position, transform.rotation);
            }
            _HBM.DestroyBounds();
            Destroy(gameObject);
        }
    }
}