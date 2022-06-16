using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SC_DiscMaster : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private LayerMask GroundLayer = 0;

    [Header("Data")]
    [SerializeField] private int attack = 0;
    [SerializeField] private float moveSpeed = 0f;

    [Header("EVENT")]
    [SerializeField] private UnityEvent OnMovementStop = new UnityEvent();
    [SerializeField] private UnityEvent OnHit = new UnityEvent();


    public int Attack { get => attack; }
    public float MoveSpeed { get => moveSpeed; }

    private bool IsInMovement = false;


    private void Start()
    {
        OnMovementStop.AddListener(() => 
        {
            rb.velocity = Vector3.zero;
            IsInMovement = false;
        });
    }

    private void Update()
    {
        if (IsInMovement)
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                OnMovementStop?.Invoke();
             
            }
        }
    }

    /*
     * Add Force to disc in the given Direction
    */
    public void LaunchDisc(Vector3 pDirection, float pForce = 1f)
    {
        pDirection.x *= pForce;
        pDirection.y = 0f;
        pDirection.z *= pForce;

        rb.velocity = pDirection;
        IsInMovement = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & GroundLayer.value) != 0)
        {
            OnHit?.Invoke();
        }
    }
}
