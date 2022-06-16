using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DiscMaster : MonoBehaviour
{
    [SerializeField] public Rigidbody rb = null;

    /*
     * Add Force to disc in the given Direction
    */
    public void LaunchDisc(Vector3 pDirection, float pForce = 1f)
    {
        pDirection.x *= pForce;
        pDirection.y = 0f;
        pDirection.z *= pForce;

        rb.velocity = pDirection;
    }
}
