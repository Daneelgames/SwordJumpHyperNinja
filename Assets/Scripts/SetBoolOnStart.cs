using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolOnStart : MonoBehaviour
{
    public Animator anim;
    public string boolTrue = "";
    public string boolFalse = "";
    // Use this for initialization
    void Start()
    {
        if (boolTrue != "")
            anim.SetBool(boolTrue, true);
        if (boolFalse != "")
            anim.SetBool(boolFalse, false);
    }
}