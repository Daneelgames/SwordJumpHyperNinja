using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool p2 = false;
    public bool gui = false;
    public PlayerAI ai;
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

    public float slowMoMeter = 3;
    public bool slowMoActive = false;
    public SlowMoMeterController slowMoMeterController;
    public AudioSource slowMoInSource;
    public AudioSource slowMoOutSource;

    public bool onPlatform = false; // if on moving platform => enforce velocity.x

    void Awake()
    {
        if (!p2)
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

        Land();
        ResetSlowMoFx();
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

        if (!dead)
        {
            Instantiate(landParticles, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            stepSource.pitch = Random.Range(0.75f, 1.25f);
            stepSource.PlayOneShot(landClip);

            if (slowMoMeterController)
                RechargeSlowMo();
        }
    }

    void RechargeSlowMo()
    {
        slowMoMeter = 1;
        slowMoMeterController.StartCoroutine("RechargeSlowMo");
        ToggleSlowMo(false);
    }

    public void JumpSoundStop()
    {
        //print ("jump sound stop");
        jumpSource.Stop();

    }

    void Update()
    {
        GetInput();
        Animate();
        SlowMo();
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
        else if (!ai)
            hSpeed = Input.GetAxis("HorizontalP2");

        if (Input.GetButtonDown("Restart") && !gui)
        {
            if (!dead && !GameManager.instance.levelClear && Time.timeSinceLevelLoad > 1f)
                GameManager.instance.RestartLevel(false, false);
        }
        else if (!ai && Input.GetButtonDown("RestartP2"))
        {
            if (!dead && !GameManager.instance.levelClear && Time.timeSinceLevelLoad > 1f)
                GameManager.instance.RestartLevel(true, false);
        }

        if (slowMoMeterController && !gui)
        {
            // slow mo
            if (Input.GetButtonDown("SlowMo"))
            {
                //start slo mo
                if (slowMoMeter > 0 && !slowMoActive && !GameManager.instance.levelClear && !grounded)

                {
                    ToggleSlowMo(true);
                    slowMoMeterController.ToggleSlowMo(true);
                }
                // input if can't start slow mo
            }
            if (Input.GetButtonUp("SlowMo"))
            {
                if (slowMoActive && !GameManager.instance.levelClear)
                {
                    //close slo mo meter
                    ToggleSlowMo(false);
                    // input if can't start slow mo
                }
            }
        }
    }

    public void ToggleSlowMo(bool active)
    {
        if (active)
        {
            slowMoActive = true;
            slowMoInSource.Play();
            slowMoOutSource.Stop();
            GameManager.instance.SetAudioPitch(true);
            //GameManager.instance.musicController.SlowMo(true);
            Time.timeScale = 0.3f;
            StartCoroutine("SlowMoPostFx", true);
        }
        else
        {
            Time.timeScale = 1;
            if (slowMoActive)
            {
                //GameManager.instance.musicController.SlowMo(false);
                GameManager.instance.SetAudioPitch(false);
                slowMoOutSource.Play();
                slowMoInSource.Stop();
                StartCoroutine("SlowMoPostFx", false);
            }
            slowMoActive = false;
        }
    }

    IEnumerator SlowMoPostFx(bool active)
    {
        var _bloom = GameManager.instance.postProcessing.profile.bloom.settings;
        var _motionBlur = GameManager.instance.postProcessing.profile.motionBlur.settings;

        print("postFx is " + active);

        if (active)
        {
            while (_bloom.bloom.intensity <= 1.2f)
            {
                _bloom.bloom.intensity += Time.unscaledDeltaTime;
                GameManager.instance.postProcessing.profile.bloom.settings = _bloom;
                _motionBlur.frameBlending += Time.unscaledDeltaTime;
                GameManager.instance.postProcessing.profile.motionBlur.settings = _motionBlur;
                if (!slowMoActive)
                {
                    StartCoroutine("SlowMoPostFx", false);
                    break;
                }
                yield return null;
            }
            if (slowMoActive)
            {
                _bloom.bloom.intensity = 1.2f;
                GameManager.instance.postProcessing.profile.bloom.settings = _bloom;
                _motionBlur.frameBlending = 1;
                GameManager.instance.postProcessing.profile.motionBlur.settings = _motionBlur;
            }
        }
        else
        {
            while (_bloom.bloom.intensity > 0.2f)
            {
                _bloom.bloom.intensity -= Time.unscaledDeltaTime;
                GameManager.instance.postProcessing.profile.bloom.settings = _bloom;
                _motionBlur.frameBlending -= Time.unscaledDeltaTime;
                GameManager.instance.postProcessing.profile.motionBlur.settings = _motionBlur;
                if (slowMoActive)
                {
                    StartCoroutine("SlowMoPostFx", true);
                    break;
                }
                yield return null;
            }
            if (!slowMoActive)
            {
                _bloom.bloom.intensity = 0.2f;
                GameManager.instance.postProcessing.profile.bloom.settings = _bloom;
                _motionBlur.frameBlending = 0;
                GameManager.instance.postProcessing.profile.motionBlur.settings = _motionBlur;
            }
        }
    }

    void ResetSlowMoFx()
    {
        var _bloom = GameManager.instance.postProcessing.profile.bloom.settings;
        var _motionBlur = GameManager.instance.postProcessing.profile.motionBlur.settings;
        _bloom.bloom.intensity = 0.2f;
        GameManager.instance.postProcessing.profile.bloom.settings = _bloom;
        _motionBlur.frameBlending = 0;
        GameManager.instance.postProcessing.profile.motionBlur.settings = _motionBlur;
    }

    void SlowMo()
    {
        if (slowMoActive)
        {
            if (slowMoMeter > 0)
            {
                slowMoMeter -= Time.unscaledDeltaTime / 3;
            }
            else
            {
                if (!GameManager.instance.levelClear)
                    ToggleSlowMo(false);
            }
        }
    }

    public void P2AIhSpeed(float _hspeed)
    {
        hSpeed = _hspeed;
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
                float enforceVelocityX = 1;
                if (onPlatform)
                    enforceVelocityX = 2;

                if (!attacking)
                    rb.velocity = new Vector2(hSpeed * enforceVelocityX * speed, rb.velocity.y);
            }
            else if (!grounded)
            {
                float p1Velocity = rb.velocity.x + Input.GetAxisRaw("Horizontal") / 8.5f;
                if (slowMoActive)
                    p1Velocity = rb.velocity.x + Input.GetAxisRaw("Horizontal") / 2;

                if (!p2) // IF P1
                    rb.velocity = new Vector2(p1Velocity, rb.velocity.y);
                else if (!ai) // IF P2
                    rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("HorizontalP2") / 8.5f, rb.velocity.y);
                else // IF P2AI
                {
                    float hSpeedNew = 0;
                    if (hSpeed > 0)
                        hSpeedNew = 1;
                    else if (hSpeed < 0)
                        hSpeedNew = -1;

                    rb.velocity = new Vector2(rb.velocity.x + hSpeedNew / 8.5f, rb.velocity.y);
                }
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

    public void ToggleOnMovingPlatform(bool _onPlatform)
    {
        onPlatform = _onPlatform;
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
        spearController.HideTarget();

        if (spearController)
            spearController.gameObject.SetActive(false);
        if (slowMoMeterController)
            slowMoMeterController.gameObject.SetActive(false);
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
                if (slowMoMeterController)
                    slowMoMeterController.ToggleSlowMo(true);
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