using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_GameHUD : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject hudPanel = null;

    [Header("DISC")]
    [SerializeField] private Text remainingDiscText = null;
    [SerializeField] private SC_ScaleAnimation remainingDiscScaleAnimation = null;
    [SerializeField] private Color DiscWarningColor = Color.red;
    [SerializeField] private float delayBetweenDiscWarning = 2f;

    [Header("GAME END")]
    [SerializeField] private TextMeshProUGUI gameEndText = null;
    [SerializeField] private GameObject gameEndPanel = null;

    // Disc Warning
    private float timeBeforePlayDiscWarning = 0f;
    private bool canPlayDiscWarning = false;

    private void Start()
    {
        remainingDiscScaleAnimation.OnScaleAnimationOver.AddListener(() =>
        {
            timeBeforePlayDiscWarning = delayBetweenDiscWarning;
        });
    }

    private void Update()
    {
        if (canPlayDiscWarning)
        {
            if (timeBeforePlayDiscWarning <= 0f)
            {
                remainingDiscScaleAnimation.StartPlayAnimation();
            }
            else
            {
                timeBeforePlayDiscWarning -= Time.deltaTime;
            }
        }
    }

    public void UpdateDiscCount(int pRemainingDisc)
    {
        remainingDiscText.text = pRemainingDisc + " DISCS LEFT";

        if (pRemainingDisc == 1)
        {
            // Play text animation
            remainingDiscScaleAnimation.StartPlayAnimation();
            remainingDiscText.color = DiscWarningColor;
            timeBeforePlayDiscWarning = 0f;
            canPlayDiscWarning = true;
        }
        else
        {
            remainingDiscText.color = Color.white;
            canPlayDiscWarning = false;
        }
    }

    public void DisplayEndGamePanel(bool pVictory)
    {
        gameEndText.text = pVictory ? "VICTORY" : "DEFEAT";
        gameEndPanel.SetActive(true);
        hudPanel.SetActive(false);
    }
}
