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
        if (other == master.coll && !master.turning)
        {
            if (master.movementType == LinearBoundsMovement.Type.Conveyor)
            {
                if (this == master.boundRight)
                    master.BoundCollide();
            }
            else
                master.BoundCollide();
        }
    }
}