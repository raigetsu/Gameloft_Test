using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SC_GameManager : MonoBehaviour
{
    [SerializeField] private SC_DiscMaster currentDisc = null;
    [SerializeField] private int discCount = 5;
    [SerializeField] private SC_GameHUD gameHUD = null;
    [SerializeField] private SC_BuildingChest chest = null;
   [SerializeField] private SC_DiscButton discLastPressedButton = null;

    private Vector3 defaultPos = Vector3.zero;

    public SC_DiscMaster CurrentDisc { get => currentDisc; }

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
        gameHUD.UpdateDiscCount(discCount);
        chest.OnChestDestroy.AddListener(Victory);
        discLastPressedButton.OnPressed();
    }

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

    public void ChangeDisc(GameObject pNewDiscPrefab)
    {
        Destroy(currentDisc.transform.parent.gameObject);
        GameObject go = Instantiate(pNewDiscPrefab);
        currentDisc = go.GetComponentInChildren<SC_DiscMaster>();
        go.transform.position = defaultPos;
        currentDisc.OnMovementStop.AddListener(OnDiscStop);
        gameState = GameState.WaitToLaunchDisc;
    }

    public void ChangeDiscWithButton(SC_DiscButton pDiscButton)
    {
        if (discLastPressedButton != null)
            discLastPressedButton.TryEnableButton();

        ChangeDisc(pDiscButton.DiscPrefab);
        discLastPressedButton = pDiscButton;
    }

    private void OnDiscStop()
    {
        if (gameState != GameState.Victory)
        {
            if (discCount <= 0)
            {
                gameState = GameState.Lose;
                gameHUD.DisplayEndGamePanel(false);
            }
            else
            {
                gameState = GameState.WaitToLaunchDisc;
            }
        }
    }

    private void Victory()
    {
        gameState = GameState.Victory;
        gameHUD.DisplayEndGamePanel(true);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
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
}
