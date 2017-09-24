using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool p2 = false;
    public Rigidbody2D rb;
    public float speed;
    public Animator anim;
    public GameObject swordSlashParticles;
    public GameObject spriteHolder;
    public bool grounded = true;
    int lookDirection = 1;
    public SpearController spearController;
    public GameObject swordBackSprite;
    public GameObject bloodParticles;
    public GameObject bloodSplatter;
    public LayerMask solidMask;
    public List<GameObject> bodyParts = new List<GameObject>();
    bool attacking = false;
    public bool dead = false;
    bool wallOnLeft = false;
    bool wallOnRight = false;
    float vSpeed = 0;
    float hSpeed;
    public Rigidbody2D healthCollider;

    public AudioSource stepSource;
    public AudioClip deathClip;
    public AudioClip landClip;
    public AudioSource jumpSource;
    public ParticleSystem jumpParticles;
    public GameObject landParticles;


    void Awake()
    {
        if(!p2)
        {
            GameManager.instance.SetPlayer(this);

            if (GameManager.instance.activeCam)
                GameManager.instance.activeCam.SetPlayer(this);
        }
        else
        {
            GameManager.instance.SetP2(this);
        }

        healthCollider.gameObject.transform.SetParent(null);

        InvokeRepeating("Stepping", 0.15f, 0.15f);
    }

    void Stepping()
    {
        if (grounded && hSpeed != 0)
        {
            stepSource.pitch = Random.Range(0.75f, 1.25f);
            stepSource.Play();
        }
    }
    void Land()
    {
        JumpSoundStop();
        var jumpEmission = jumpParticles.emission;
        jumpEmission.rateOverTime = 0;

        Instantiate(landParticles, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        stepSource.pitch = Random.Range(0.75f, 1.25f);
        stepSource.PlayOneShot(landClip);
    }

    public void JumpSoundStop()
    {
        print ("jump sound stop");
        jumpSource.Stop();

    }

    void Update()
    {
        GetInput();
        Animate();

        //set jump sfx pitch
        if (!grounded)
            jumpSource.pitch -= 0.1f * Time.deltaTime;
    }

    void FixedUpdate()
    {
        ApplyForces();
        CheckGround();

        healthCollider.MovePosition(transform.position);
    }

    void GetInput()
    {
        if (!p2)
            hSpeed = Input.GetAxis("Horizontal");
        else
            hSpeed = Input.GetAxis("HorizontalP2");

		if (Input.GetButtonDown("Restart"))
        {
			if (!dead && !GameManager.instance.levelClear && Time.timeSinceLevelLoad > 1f)
                GameManager.instance.RestartLevel(false);
        }
		else if (Input.GetButtonDown("RestartP2"))
        {
			if (!dead && !GameManager.instance.levelClear && Time.timeSinceLevelLoad > 1f)
                GameManager.instance.RestartLevel(true);
        }
    }

    public void SetBound(string side, bool inTrigger)
    {
        switch (side)
        {
            case "Left":
                wallOnLeft = inTrigger;
                break;
            case "Right":
                wallOnRight = inTrigger;
                break;
        }
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
        if (!dead)
        {
            if (grounded)
            {
                if (!attacking)
                    rb.velocity = new Vector2(hSpeed * speed, rb.velocity.y);
            }
            else if (!grounded)
            {
                if (!p2)
                    rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("Horizontal") / 8.5f, rb.velocity.y);
                    else
                    rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("HorizontalP2") / 8.5f, rb.velocity.y);
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

            if (newVel.x > 0 && wallOnRight)
                newVel = new Vector2(0, rb.velocity.y);
            else if (newVel.x < 0 && wallOnLeft)
                newVel = new Vector2(0, rb.velocity.y);

            rb.velocity = newVel;
        }
        //else
        // rb.velocity = Vector2.zero;
    }

    void Animate()
    {
        if (!dead)
        {
            if (grounded)
            {
                anim.SetBool("Jump", false);
                //if (rb.velocity != Vector2.zero)
                if (hSpeed != 0)
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
    }

    public void Dead()
    {
        dead = true;

        stepSource.pitch = 1;
        stepSource.PlayOneShot(deathClip);

        JumpSoundStop();

        if (GameManager.instance.activeCam)
            GameManager.instance.activeCam.SetTrigger("ShakeBig");

        foreach (GameObject c in bodyParts)
        {
            GameObject part = Instantiate(c, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
        anim.gameObject.SetActive(false);
        spearController.gameObject.SetActive(false);
    }

    void CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.6f, solidMask))
        {
            if (!grounded)
                Land();

            grounded = true;
            JumpSoundStop();
        }
        else
        {
            if (grounded)
            {
                JumpSoundStop();
                jumpSource.pitch = Random.Range(1f, 1.25f);
                jumpSource.Play();

                var jumpEmission = jumpParticles.emission;
                jumpEmission.rateOverTime = 50;
            }
            grounded = false;
        }
    }

    public void Bounce(Vector3 bouncePos) // sword attack
    {
        StartCoroutine("AnimateSword");

        if ((bouncePos.y > transform.position.y && rb.velocity.y > 0) || (bouncePos.y < transform.position.y && rb.velocity.y < 0))
            rb.velocity = new Vector2(rb.velocity.x, 0);

        JumpSoundStop();

        if (!GameManager.instance.levelClear)
        {
            jumpSource.pitch = Random.Range(1f, 1.25f);
            jumpSource.Play();
        }

        rb.AddForce((transform.position - bouncePos) * 10, ForceMode2D.Impulse);
    }

    IEnumerator AnimateSword()
    {
        GameObject slash = Instantiate(swordSlashParticles, transform.position, transform.rotation) as GameObject;
        slash.transform.SetParent(spearController.gameObject.transform);
        slash.transform.localPosition = new Vector2(0.5f, 0);
        slash.transform.localEulerAngles = new Vector3(0, 0, 270);
        slash.transform.SetParent(null);
        swordBackSprite.SetActive(false);
        attacking = true;
        yield return new WaitForSecondsRealtime(0.25f);
        attacking = false;
        swordBackSprite.SetActive(true);
    }

    void OnDestroy()
    {
        print("player destroyed");
    }
}