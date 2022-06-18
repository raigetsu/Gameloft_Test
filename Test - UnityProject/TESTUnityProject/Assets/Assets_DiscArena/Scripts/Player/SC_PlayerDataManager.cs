using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SC_PlayerDataManager : MonoBehaviour
{
    [SerializeField] private SCO_DiscData defaultDiscData = null;
    public UnityEvent<SC_Rank.ERank, int> OnRankUp = new UnityEvent<SC_Rank.ERank, int>();

    [System.Serializable]
    class PlayerSave
    {
        public SC_Rank.ERank currentRank = SC_Rank.ERank.Bronze;
        public int rankLevel = 3;
        public int rowCount = 0;
        public List<string> unlockedDisc = new List<string>();
    }

    // Data of all disc
    private List<SCO_DiscData> discDatas = new List<SCO_DiscData>();

    // player Data
    SC_Rank.ERank currentRank = SC_Rank.ERank.Bronze;
    int rankLevel = 3;
    int rowCount = 0;
    public List<SCO_DiscData> UnlockedDisc { get; private set; } = new List<SCO_DiscData>();

    // GETTER
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

            LoadDiscAddressables(playerSave);
        }
        else
        {
            UnlockedDisc.Clear();
            UnlockedDisc.Add(defaultDiscData);
            Save();
            LoadDiscAddressables(null);
        }
    }

    public void Save()
    {
        PlayerSave save = new PlayerSave();
        save.rankLevel = rankLevel;
        save.currentRank = currentRank;
        save.rowCount = rowCount;
        for (int i = 0; i < UnlockedDisc.Count; i++)
        {
            save.unlockedDisc.Add(UnlockedDisc[i].AddressablesKey);
        }

        string json = JsonUtility.ToJson(save);

        File.WriteAllText(Application.persistentDataPath + "/PlayerSave.json", json);
    }

    public void Victory()
    {
        rowCount++;
        if (rowCount >= SC_Rank.RowToRankUp(currentRank, rankLevel))
        {
            rowCount = 0;

            if (((int)currentRank != (int)SC_Rank.ERank.Count - 1 && rankLevel == 1) ||
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

                // Check if we unlocked new disc
                for (int i = 0; i < discDatas.Count; i++)
                {
                    if (discDatas[i].UnlockedRank == currentRank && discDatas[i].UnlockedRankLevel == rankLevel)
                    {
                        if (UnlockedDisc.Contains(discDatas[i]) == false)
                        {
                            UnlockedDisc.Add(discDatas[i]);
                        }
                    }
                }
            }
        }

        Save();
    }

    public void Lose()
    {
        rowCount = 0;
        Save();
    }

    private void LoadDiscAddressables(PlayerSave playerSave)
    {
        var operation = Addressables.InitializeAsync();
        SC_LoadingScreen.Instance.AddASyncOperationHandle(operation);

        operation.Completed += (obj) =>
        {
            var allDiscDataOperation = Addressables.LoadAssetsAsync<SCO_DiscData>("Disc", (pDiscDatas) =>
            {
                discDatas.Add(pDiscDatas);
            });

            SC_LoadingScreen.Instance.AddASyncOperationHandle(allDiscDataOperation);

            // Load disc with save
            if (playerSave != null)
            {
                // Get all unlocked disc data
                allDiscDataOperation.Completed += (obj) =>
                {
                    for (int i = 0; i < playerSave.unlockedDisc.Count; i++)
                    {
                        for (int j = 0; j < discDatas.Count; j++)
                        {
                            if (playerSave.unlockedDisc[i] == discDatas[j].AddressablesKey)
                            {
                                UnlockedDisc.Add(discDatas[j]);
                                break;
                            }
                        }
                    }
                };
            }
        };
    }
}