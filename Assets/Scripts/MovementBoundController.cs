using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoundController : MonoBehaviour
{
    LinearBoundsMovement master;

    public void SetMaster(LinearBoundsMovement _master)
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