using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DiscBomber : SC_DiscMaster
{
    [SerializeField] private int explodeDamage = 5;
    [SerializeField] private float explosionRange = 5f;
    [SerializeField] private GameObject explodeFx = null;
    [SerializeField, Tooltip("-1 = unlimited")] private int possibleExplosionCount = 1;

    private void Start()
    {
        FindObjectOfType<SC_PlayerInput>().OnInputTouch.AddListener(Explode);
    }

    private void Explode()
    {
        if (possibleExplosionCount > 0 || possibleExplosionCount == -1)
        {
            GameObject explosionFx = Instantiate(explodeFx);
            explosionFx.transform.position = transform.position;

            Collider[] hit = Physics.OverlapSphere(transform.position, explosionRange);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].gameObject.CompareTag("Building"))
                {
                    hit[i].gameObject.GetComponent<SC_BuildingMaster>().TakeDamage(explodeDamage);
                }
            }

            possibleExplosionCount -= 1;
        }
    }

    private void OnDestroy()
    {
        SC_PlayerInput playerInput = FindObjectOfType<SC_PlayerInput>();
        if (playerInput != null)
        {
            playerInput.OnInputTouch.RemoveListener(Explode);
        }
    }
}
