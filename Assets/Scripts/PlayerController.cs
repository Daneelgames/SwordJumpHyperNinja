using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Animator anim;
    public GameObject swordSlashParticles;
    public GameObject spriteHolder;
    public bool grounded = true;
    int lookDirection = 1;
    public SpearController spearController;
    public GameObject swordBackSprite;
    public LayerMask solidMask;
    bool attacking = false;
    float vSpeed = 0;
    float hSpeed;


    void Update()
    {
        GetInput();
        Animate();
    }

    void FixedUpdate()
    {
        ApplyForces();
        CheckGround();
    }

    void GetInput()
    {
        hSpeed = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown("r"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void LookDirection()
    {
        if (grounded)
        {
            if (hSpeed > 0)
                lookDirection = 1;
            else if (hSpeed < 0)
                lookDirection = -1;

            gameObject.transform.localScale = new Vector3(lookDirection, 1, 1);
        }
    }
    void ApplyForces()
    {
        if (grounded && !attacking)
        {
            rb.velocity = new Vector2(hSpeed * speed, rb.velocity.y);
        }
        else if (!grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("Horizontal")/8.5f, rb.velocity.y);
        }
        var newVel = rb.velocity;

        if (newVel.y > 7)
            newVel.y = 7;
        else if (newVel.y < -15)
            newVel.y = -15;

        if (newVel.x > 7)
            newVel.x = 7;
        else if (newVel.x < -7)
            newVel.x = -7;

        rb.velocity = newVel;
    }

    void Animate()
    {
        if (grounded)
        {
            anim.SetBool("Jump", false);
            if (rb.velocity != Vector2.zero)
                anim.SetBool("Move", true);
            else
                anim.SetBool("Move", false);
        }
        else
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Move", false);
        }
        if (rb.velocity.x > 0)
        {
            spriteHolder.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.velocity.x < 0)
        {
            spriteHolder.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.6f, solidMask))
            grounded = true;
        else
            grounded = false;
    }

    public void Bounce(Vector3 bouncePos) // sword attack
    {
        StartCoroutine("AnimateSword");

        if ((bouncePos.y > transform.position.y && rb.velocity.y > 0) || (bouncePos.y < transform.position.y && rb.velocity.y < 0))
            rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce((transform.position - bouncePos) * 10, ForceMode2D.Impulse);
    }

    IEnumerator AnimateSword()
    {
        //instantiate temp particles
        GameObject slash = Instantiate(swordSlashParticles, transform.position, transform.rotation) as GameObject;
        slash.transform.SetParent(spearController.gameObject.transform);
        slash.transform.localPosition = new Vector2(0.5f, 0);
        slash.transform.localEulerAngles = new Vector3(0, 0, 270);
        slash.transform.SetParent(transform);
        swordBackSprite.SetActive(false);
        attacking = true;
        yield return new WaitForSeconds(0.25f);
        attacking = false;
        swordBackSprite.SetActive(true);
    }
}