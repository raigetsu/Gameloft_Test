using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_BuildingMaster : MonoBehaviour
{
    [SerializeField] private int health = 0;
    [SerializeField] private GameObject textDamagePrefab = null;
    [SerializeField] private Vector3 damageTextSpawnPositionOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private SC_HealthBar healthBar = null;

    [Header("Scale Animation")]
    [SerializeField] private float scaleAnimationFactor = 1.2f;
    [SerializeField] private float scaleAnimationSpeed = 2f;


    public float currentHealth { get; private set; } = 0;

    // Scale Animation Data
    private bool isPlayingScaleAnimation = false;
    private float currentScaleFactor = 1f;
    private Vector3 defaulScale = Vector3.one;
    private bool isPlayingReverseAnimation = false;

    private void Start()
    {
        currentHealth = health;
    }

    private void Update()
    {
        if (isPlayingScaleAnimation)
        {
            UpdateScaleAnimation();   
        }
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
            // Start playing scale animation
            if (isPlayingScaleAnimation == false)
            {
                isPlayingReverseAnimation = false;
                isPlayingScaleAnimation = true;
                defaulScale = gameObject.transform.localScale;
            }
        }

        return false;
    }

    public virtual void BuildingDestroy()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        Destroy(gameObject.transform.parent.gameObject, 0.1f);
    }

    private void UpdateScaleAnimation()
    {
        if (isPlayingReverseAnimation == false)
        {
            currentScaleFactor += scaleAnimationFactor * Time.deltaTime * scaleAnimationSpeed;
            if (currentScaleFactor >= scaleAnimationFactor)
            {
                isPlayingReverseAnimation = true;
            }
        }
        else
        {
            currentScaleFactor -= scaleAnimationFactor * Time.deltaTime * scaleAnimationSpeed;
            if (currentScaleFactor <= 1f)
                isPlayingScaleAnimation = false;
        }

        gameObject.transform.localScale = defaulScale * currentScaleFactor;
    }
}