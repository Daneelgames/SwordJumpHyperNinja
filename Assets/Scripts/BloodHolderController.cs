using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodHolderController : MonoBehaviour
{
    public GameObject bloodLoop;
    public GameObject bloodShot;
    public void EmitBlood()
    {
        GameObject newBlood = Instantiate(bloodLoop, transform.position, Quaternion.identity) as GameObject;
        newBlood.transform.SetParent(transform);
        newBlood.transform.localScale = Vector3.one;
        newBlood.transform.LookAt(GameManager.instance.pc.transform.position);

        GameObject newbloodShot = Instantiate(bloodShot, transform.position, Quaternion.identity) as GameObject;
        newbloodShot.transform.SetParent(transform);

        Vector3 moveDirection = gameObject.transform.position - GameManager.instance.pc.transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        newbloodShot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //newbloodShot.transform.LookAt(GameManager.instance.pc.transform.position);
        newbloodShot.transform.localScale = Vector3.one;
    }
}