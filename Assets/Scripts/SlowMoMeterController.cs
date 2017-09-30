using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowMoMeterController : MonoBehaviour
{
    public PlayerController pc;
    public Image slowMoSprite;
    public Animator anim;

    void Update()
    {
        Animate();

        if (pc.slowMoActive)
            slowMoSprite.fillAmount -= Time.unscaledDeltaTime / 3;
    }

    public void ToggleSlowMo(bool active)
    {
        anim.SetBool("Active", active);
    }

    void Animate()
    {
        float g = 255f;
        float b = 255f;

        slowMoSprite.color = new Color(255, g, b, 100);
    }
    IEnumerator RechargeSlowMo()
    {
        anim.SetTrigger("Recharge");
        anim.SetBool("Active", false);
        slowMoSprite.fillAmount = 0.1f;

        while (slowMoSprite.fillAmount < 1)
        {
            slowMoSprite.fillAmount += 3 * Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
