using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SC_PlayerDataManager : MonoBehaviour
{
    public UnityEvent<SC_Rank.ERank, int> OnRankUp = new UnityEvent<SC_Rank.ERank, int>();

    [System.Serializable]
    class PlayerSave
    {
        public SC_Rank.ERank currentRank = SC_Rank.ERank.Bronze;
        public int rankLevel = 3;
        public int rowCount = 0;
    }

    SC_Rank.ERank currentRank = SC_Rank.ERank.Bronze;
    int rankLevel = 3;
    int rowCount = 0;

    public SC_Rank.ERank CurrentRank { get => currentRank; }
    public int RankLevel { get => rankLevel; }
    public int RowCount { get => rowCount; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        string save = Application.persistentDataPath + "/PlayerSave.json";

        if (File.Exists(save))
        {
            string json = File.ReadAllText(save);
            PlayerSave playerSave = JsonUtility.FromJson<PlayerSave>(json);

            currentRank = playerSave.currentRank;
            rankLevel = playerSave.rankLevel;
            rowCount = playerSave.rowCount;

            print("Load");
        }
    }

    public void Save()
    {
        PlayerSave save = new PlayerSave();
        save.rankLevel = rankLevel;
        save.currentRank = currentRank;
        save.rowCount = rowCount;

        string json = JsonUtility.ToJson(save);

        File.WriteAllText(Application.persistentDataPath + "/PlayerSave.json", json);
    }

    public void Victory()
    {
        rowCount++;
        if (rowCount >= SC_Rank.RowToRankUp(currentRank, rankLevel))
        {
            rowCount = 0;

            if ((int)currentRank != (int)SC_Rank.ERank.Count - 1 &&
                rankLevel != 1)
            {
                if (rankLevel == 1)
                {
                    rankLevel = 3;
                    int _CurrentRank = (int)currentRank + 1;
                    currentRank = (SC_Rank.ERank)_CurrentRank;
                }
                else
                {
                    rankLevel--;
                }

                OnRankUp?.Invoke(currentRank, rankLevel);
            }
        }
        print(rankLevel);
        Save();
    }

    public void Lose()
    {
        rowCount = 0;
        Save();
    }
}
