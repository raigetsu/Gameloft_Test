using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SC_ScaleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject objectToScale = null;
    [Header("Scale Animation")]
    [SerializeField] private float scaleAnimationFactor = 1.2f;
    [SerializeField] private float scaleAnimationSpeed = 2f;
    [SerializeField] public UnityEvent OnScaleAnimationOver = new UnityEvent();

    public float currentHealth { get; private set; } = 0;

    // Scale Animation Data
    private bool isPlayingScaleAnimation = false;
    private float currentScaleFactor = 1f;
    private Vector3 defaulScale = Vector3.one;
    private bool isPlayingReverseAnimation = false;

    private void Update()
    {
        if(isPlayingScaleAnimation)
        {
            UpdateScaleAnimation();
        }
    }

    public void StartPlayAnimation()
    {
        if (isPlayingScaleAnimation == false)
        {
            defaulScale = objectToScale.transform.localScale;
            isPlayingScaleAnimation = true;
            isPlayingReverseAnimation = false;
        }
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
            {
                isPlayingScaleAnimation = false;
                OnScaleAnimationOver?.Invoke();
            }
        }

        objectToScale.transform.localScale = defaulScale * currentScaleFactor;
    }
}
