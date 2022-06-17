using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_DiscButton : MonoBehaviour
{
    [SerializeField,Tooltip("Set -1 for unlimited use")] private int usableCount = -1;
    [SerializeField] private Button button = null;
    [SerializeField] private GameObject discPrefab = null;

    public GameObject DiscPrefab { get => discPrefab; }

    private int currentUsedCount = 0;

    public void IncreaseUsedCount()
    {
        currentUsedCount++;
    }

    public void OnPressed()
    {
        button.interactable = false;
    }

    public void TryEnableButton()
    {
        if (usableCount != -1)
            button.interactable = currentUsedCount >= usableCount;
        else
            button.interactable = true;
    }
}
