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
        public SC_MoveObject.SaveMoveObject moveObjectSave = null;
        public SC_RotateObject.SaveRotateObject rotateObjectSave = null;
    }

    [SerializeField] private int health = 0;
    [SerializeField] private GameObject textDamagePrefab = null;
    [SerializeField] private Vector3 damageTextSpawnPositionOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] protected SC_HealthBar healthBar = null;
    [SerializeField] private SC_ScaleAnimation scaleAnimation = null;
    [SerializeField] private bool isInvincible = false;

    [Header("PARTICLE SYSTEM")]
    [SerializeField] private ParticleSystem fireParticle = null;
    [SerializeField] private GameObject smokeParticle = null;

    [Header("Save")]
    [SerializeField] private string key = "BuildingMaster";
    [SerializeField, Tooltip("Keep null if no use")] private SC_MoveObject moveObject = null;
    [SerializeField, Tooltip("Keep null if no use")] private SC_RotateObject rotateObject = null;

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
        if (isInvincible)
        {

            return false;
        }

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
            healthBar.UpdateHealth((float)currentHealth / (float)health);
        }

        if (currentHealth <= 0)
        {
            BuildingDestroy();
            return true;
        }
        else
        {
            scaleAnimation.StartPlayAnimation();

            if (fireParticle != null)
            {
                if (fireParticle.isPlaying == false)
                {
                    fireParticle.Play();
                }
            }
        }

        return false;
    }

    public virtual void BuildingDestroy()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        GameObject go = Instantiate(smokeParticle);
        go.transform.position = transform.position;
        Destroy(gameObject.transform.parent.gameObject, 0.1f);
    }

    public void LoadSave(BuildingSave save)
    {
        health = save.health;
        currentHealth = health;

        if (save.moveObjectSave.checkpoint.Length > 1)
        {
            moveObject = gameObject.transform.parent.gameObject.AddComponent<SC_MoveObject>();
            moveObject.LoadSave(save.moveObjectSave);
        }

        if (save.rotateObjectSave.useRotateObject == true)
        {
            rotateObject = gameObject.transform.parent.gameObject.AddComponent<SC_RotateObject>();
            rotateObject.LoadSave(save.rotateObjectSave);
        }

        if(isInvincible)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    public BuildingSave Save()
    {
        BuildingSave save = new BuildingSave();

        save.key = Key;
        save.position = transform.parent.transform.position;
        save.rotation = transform.parent.transform.rotation;
        save.scale = transform.parent.transform.localScale;
        save.health = MaxHealth;

        if (moveObject != null)
            save.moveObjectSave = moveObject.Save();
        else
            save.moveObjectSave = null;

        if (rotateObject != null)
            save.rotateObjectSave = rotateObject.Save();
        else
            save.rotateObjectSave = null;

        return save;
    }
}