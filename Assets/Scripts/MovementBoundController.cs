using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoundController : MonoBehaviour
{
    HorizontalBoundsMovement master;

    public void SetMaster(HorizontalBoundsMovement _master)
    {
        master = _master;
		transform.SetParent(null);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == master.coll)
        {
            master.ChangeDirection();
        }
    }
}