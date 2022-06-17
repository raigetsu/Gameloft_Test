using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SC_GameManager : MonoBehaviour
{
    [SerializeField] private SC_DiscMaster currentDisc = null;
    [SerializeField] private int discCount = 5;
    [SerializeField] private SC_GameHUD gameHUD = null;
    [SerializeField] private SC_BuildingChest chest = null;
    [SerializeField] private SC_DiscButton discLastPressedButton = null;

    private Vector3 defaultPos = Vector3.zero;
    SC_PlayerDataManager playerDataManager = null;

    public SC_DiscMaster CurrentDisc { get => currentDisc; }
    public int DiscCount { get => discCount; }

    public enum GameState
    {
        WaitToChooseDisc,
        WaitToLaunchDisc,
        DiscIsMoving,
        Victory,
        Lose
    }

    public GameState gameState { get; private set; } = GameState.WaitToLaunchDisc;

    private void Start()
    {
        defaultPos = currentDisc.transform.position;
        currentDisc.OnMovementStop.AddListener(OnDiscStop);

        // Bind victory
        if (chest != null)
            chest.OnChestAnimationOver.AddListener(Victory);

        // Load level
        playerDataManager = FindObjectOfType<SC_PlayerDataManager>();
        SC_LevelGeneration.LoadLevel(SC_Rank.GetLevelName(playerDataManager.CurrentRank, playerDataManager.RankLevel, playerDataManager.RowCount));
    }

    #region DISC
    public void LaunchDisc(Vector3 pDirection, float pForce = 1f)
    {
        currentDisc.LaunchDisc(pDirection, pForce);
        gameState = GameState.DiscIsMoving;
        discCount--;
        gameHUD.UpdateDiscCount(discCount);
        discLastPressedButton.IncreaseUsedCount();
    }

    public Vector3 GetDiscPosition()
    {
        return currentDisc.gameObject.transform.position;
    }

    #region DISC CHANGEMENT
    public void ChangeDisc(GameObject pNewDiscPrefab)
    {
        Destroy(currentDisc.transform.parent.gameObject);

        GameObject go = Instantiate(pNewDiscPrefab);

        currentDisc = go.GetComponentInChildren<SC_DiscMaster>();
        go.transform.position = defaultPos;

        currentDisc.OnMovementStop.AddListener(OnDiscStop);
        gameState = GameState.WaitToLaunchDisc;
    }

    // Call Change Disc
    // Update button state if necessary
    public void ChangeDiscWithButton(SC_DiscButton pDiscButton)
    {
        if (discLastPressedButton != null && discLastPressedButton != pDiscButton)
            discLastPressedButton.TryEnableButton();

        ChangeDisc(pDiscButton.data.Prefab);
        discLastPressedButton = pDiscButton;

        gameState = GameState.WaitToLaunchDisc;
    }
    #endregion

    private void OnDiscStop()
    {
        if (gameState != GameState.Victory)
        {
            if (discCount <= 0)
            {
                gameState = GameState.Lose;
                playerDataManager.Lose();
                gameHUD.DisplayEndGamePanel(false, playerDataManager);
            }
            else
            {
                gameState = GameState.WaitToChooseDisc;
                gameHUD.DiscStopped();
            }
        }
    }

    public void NewDiscButtonCreated(SC_DiscButton pDiscButton)
    {
        pDiscButton.DiscButton.onClick.AddListener(() => { ChangeDiscWithButton(pDiscButton); });
        if (discLastPressedButton == null)
        {
            discLastPressedButton = pDiscButton;
        }
    }
    #endregion

    private void Victory()
    {
        gameState = GameState.Victory;
        playerDataManager.Victory();
        gameHUD.DisplayEndGamePanel(true, playerDataManager);
    }

    public void GoBackToMenu()
    {
        Destroy(playerDataManager.gameObject);
        SceneManager.LoadScene(0);
    }

    // Return if mouse/Touch(0) is over UI Element 
    public bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
        eventData.position = Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            eventData.position = new Vector3(Input.GetTouch(0).position.x, 0f, Input.GetTouch(0).position.y);
        else
            return false;
#endif
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];
            if (curRaysastResult.gameObject.layer == 5)
                return true;
        }

        return false;
    }

    public void LoadLevel(SC_LevelGeneration.LevelData data)
    {
        // Bind Victory
        chest = FindObjectOfType<SC_BuildingChest>();
        chest.OnChestAnimationOver.AddListener(Victory);

        // Setup disc max count
        discCount = data.levelDiscCount;
        gameHUD.UpdateDiscCount(discCount);

        // Generate disc button
        gameHUD.InitDiscButton(playerDataManager.UnlockedDisc, this);
        discLastPressedButton.OnPressed();

        // Setup level creator name
        gameHUD.UpdateCreatorName(data.creatorName);
    }
}
