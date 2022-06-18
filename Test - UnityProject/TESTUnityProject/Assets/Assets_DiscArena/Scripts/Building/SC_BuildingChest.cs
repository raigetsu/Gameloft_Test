using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class SC_BuildingChest : SC_BuildingMaster
{
    [Header("CHEST")]
    [SerializeField] private Animation chestOpeningAnimation = null;
    [SerializeField] private CinemachineVirtualCamera chestCam = null;
    [SerializeField] private ParticleSystem starParticle = null;
    [SerializeField] public UnityEvent OnChestAnimationOver = new UnityEvent();

    private bool isPlayingOpeningAnimation = false;

    private void Update()
    {
        if (isPlayingOpeningAnimation)
        {
            if (chestOpeningAnimation.isPlaying == false)
            {
                isPlayingOpeningAnimation = false;
                OnChestAnimationOver?.Invoke();
                chestCam.Priority = 10;
            }
        }
    }

    public override void BuildingDestroy()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        chestOpeningAnimation.Play();
        isPlayingOpeningAnimation = true;
        chestCam.Priority = 30;
        healthBar.gameObject.SetActive(false);
    }

    public void PlayStarFx()
    {
        starParticle.Play();
    }
}
