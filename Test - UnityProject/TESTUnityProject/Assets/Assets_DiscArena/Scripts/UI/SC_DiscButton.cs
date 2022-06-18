using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_DiscButton : MonoBehaviour
{

    [SerializeField] private Button button = null;
    [SerializeField] private Image discIcon = null;
    [SerializeField] private SC_ScaleAnimation scaleAnimation = null;

    public SCO_DiscData data { get; private set; } = null;
    public Button DiscButton { get => button; }

    private int currentUsedCount = 0;

    public void IncreaseUsedCount()
    {
        currentUsedCount++;
    }

    public void OnPressed()
    {
        button.interactable = false;
    }

    public bool TryEnableButton()
    {
        if (data.UsableCount != -1)
        {
            button.interactable = currentUsedCount < data.UsableCount;
        }
        else
        {
            button.interactable = true;
        }

        return button.interactable;
    }

    public void InitButton(SCO_DiscData pDisc)
    {
        data = pDisc;
        discIcon.sprite = pDisc.IconTexture;
    }

    public void PlayScaleAnimation()
    {
        scaleAnimation.StartPlayAnimation();
    }
}
