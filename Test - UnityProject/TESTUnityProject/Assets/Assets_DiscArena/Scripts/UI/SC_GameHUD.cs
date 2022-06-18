using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_GameHUD : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject hudPanel = null;
    [SerializeField] private Text levelCreateNameText = null;

    [Header("DISC")]
    [SerializeField] private GameObject discButtonPrefab = null;
    [SerializeField] private GameObject discButtonLayout = null;
    [SerializeField] private GameObject discInformationPanel = null;
    [SerializeField] private Text discNameText = null;
    [SerializeField] private Text discInformationText = null;

    [Header("DISC COUNT")]
    [SerializeField] private Text remainingDiscText = null;
    [SerializeField] private SC_ScaleAnimation remainingDiscScaleAnimation = null;
    [SerializeField] private Color DiscWarningColor = Color.red;
    [SerializeField] private float delayBetweenDiscWarning = 2f;

    [Header("GAME END")]
    [SerializeField] private TextMeshProUGUI gameEndText = null;
    [SerializeField] private GameObject gameEndPanel = null;
    [SerializeField] private Text rankText = null;

    // Disc Warning
    private float timeBeforePlayDiscWarning = 0f;
    private bool canPlayDiscWarning = false;
    private List<SC_DiscButton> discButtonList = new List<SC_DiscButton>();

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

    public void DisplayEndGamePanel(bool pVictory, SC_PlayerDataManager pPlayerData)
    {
        gameEndText.text = pVictory ? "VICTORY" : "DEFEAT";
        gameEndPanel.SetActive(true);
        hudPanel.SetActive(false);

        if (pVictory)
        {
            if (pPlayerData.RowCount != 0)
                rankText.text = "STILL <color=orange>" + (SC_Rank.RowToRankUp(pPlayerData.CurrentRank, pPlayerData.RankLevel) - pPlayerData.RowCount) + "</color> GAMES IN A ROW\nTO RANK UP";
            else
                rankText.text = "PROMOTED TO <color=orange>NEW LEAGUE!</color>";
        }
        else
        {
            rankText.text = "RESET STREAK";
        }
    }

    public void UpdateCreatorName(string pName)
    {
        levelCreateNameText.text = pName;
    }

    public void InitDiscButton(List<SCO_DiscData> pUnlockedDisc, SC_GameManager pGameManager)
    {
        for (int i = 0; i < pUnlockedDisc.Count; i++)
        {
            GameObject go = Instantiate(discButtonPrefab, discButtonLayout.transform);

            SC_DiscButton discButton = go.GetComponent<SC_DiscButton>();

            // Init button
            discButton.InitButton(pUnlockedDisc[i]);
            pGameManager.NewDiscButtonCreated(discButton);

            // Save button in list
            discButtonList.Add(discButton);
        }
    }

    public void DiscStopped()
    {
        for (int i = 0; i < discButtonList.Count; i++)
        {
            if (discButtonList[i].TryEnableButton())
            {
                discButtonList[i].PlayScaleAnimation();
            }
        }
    }

    public void ActivateAllButton()
    {
        for (int i = 0; i < discButtonList.Count; i++)
        {
            discButtonList[i].TryEnableButton();
        }
    }

    public void DisplayDiscInformation(bool pDisplayinformation, SCO_DiscData pDiscData = null)
    {
        if (pDisplayinformation)
        {
            discInformationPanel.SetActive(true);
            discNameText.text = pDiscData.DiscName;
            discInformationText.text = pDiscData.DiscInformation;
        }
        else
        {
            discInformationPanel.SetActive(false);
        }
    }
}
