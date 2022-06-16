using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DiscMaster : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Add Force to disc in the given Direction
    */
    void LaunchDisc(Vector3 Direction, float Force = 1f)
    {
        Direction.x *= Force;
        Direction.y *= Force;
        Direction.z *= Force;

        rigidbody.AddForce(Direction);
    }
}
