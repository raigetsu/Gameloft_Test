using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SC_MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText = null;
    [SerializeField] private Text rowText = null;
    [SerializeField] private SC_PlayerDataManager playerData = null;

    private void Start()
    {
        string rankString = SC_Rank.GetRankString(playerData.CurrentRank).ToUpper() + " ";
        for (int i = 0; i < playerData.RankLevel; i++)
        {
            rankString += "I";
        }
        rankText.text = rankString;

        rowText.text = "WIN " + (SC_Rank.RowToRankUp(playerData.CurrentRank, playerData.RankLevel) - playerData.RowCount) + " GAMES IN A ROW TO RANK UP";
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene(1);
    }
}
