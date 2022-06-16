using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DiscMaster : MonoBehaviour
{
    [SerializeField] public Rigidbody rb = null;
    [SerializeField] private LayerMask GroundLayer;

    // Start is called before the first frame update
    void Start()
    {
        LaunchDisc(new Vector3(1f, 0f, -1f), 200f);
    }

    // Update is called once per frame
    void Update()
    {
        print("<color=green>" + rb.velocity+ "</color>");
    }

    /*
     * Add Force to disc in the given Direction
    */
    void LaunchDisc(Vector3 Direction, float Force = 1f)
    {
        Direction.x *= Force;
        Direction.y *= Force;
        Direction.z *= Force;

        rb.velocity = new Vector3(4, 0f,-4);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != GroundLayer.value)
        {

        print(collision.gameObject.layer);
        print("<color=red>" + rb.velocity+ "</color>");
        }
    }

 
}
