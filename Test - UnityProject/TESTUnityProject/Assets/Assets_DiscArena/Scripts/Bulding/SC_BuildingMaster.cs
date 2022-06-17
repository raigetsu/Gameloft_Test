using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_BuildingMaster : MonoBehaviour
{
    [SerializeField] private int health = 0;

    // Return true if the building is destroy
    public bool TakeDamage(int damage)
    {
        health -= damage;
        print(health);
        if (health <= 0)
        {
            BuildingDestroy();
            return true;
        }
        return false;
    }

    public virtual void BuildingDestroy()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        Destroy(gameObject,0.1f);
    }
}
