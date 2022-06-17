using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_BuildingMaster : MonoBehaviour
{
    [System.Serializable]
    public class BuildingSave
    {
        public string key = null;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public int health = 0;
    }

    [SerializeField] private int health = 0;
    [SerializeField] private GameObject textDamagePrefab = null;
    [SerializeField] private Vector3 damageTextSpawnPositionOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private SC_HealthBar healthBar = null;
    [SerializeField] private SC_ScaleAnimation scaleAnimation = null;

    [Header("Save")]
    [SerializeField] private string key = "BuildingMaster";

    public string Key { get => key; }
    public int currentHealth { get; private set; } = 0;
    public int MaxHealth { get => health; }

    private void Start()
    {
        currentHealth = health;
    }

    // Return true if the building is destroy
    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Spawn Text Damage 
        if (textDamagePrefab != null)
        {
            GameObject go = Instantiate(textDamagePrefab);
            go.transform.position = gameObject.transform.position + damageTextSpawnPositionOffset;
            go.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString());
        }
        else
        {
            Debug.LogWarning(name + " has non valid text prefab");
        }

        // Update Health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth / health);
        }

        if (currentHealth <= 0)
        {
            BuildingDestroy();
            return true;
        }
        else
        {
            scaleAnimation.StartPlayAnimation();
        }

        return false;
    }

    public virtual void BuildingDestroy()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        Destroy(gameObject.transform.parent.gameObject, 0.1f);
    }

    public void LoadSave(BuildingSave save)
    {
        health = save.health;
        currentHealth = health;
    }
}