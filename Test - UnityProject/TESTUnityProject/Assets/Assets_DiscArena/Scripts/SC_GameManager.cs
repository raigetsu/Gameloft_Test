using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    [SerializeField] private SC_DiscMaster currentDisc = null;
    private Vector3 defaultPos = Vector3.zero;

    public SC_DiscMaster CurrentDisc { get => currentDisc; }

    public enum GameState
    {
        WaitToLaunchDisc,
        DiscIsMoving
    }

    public GameState gameState { get; private set; } = GameState.WaitToLaunchDisc;

    private void Start()
    {
        defaultPos = currentDisc.transform.position;
    }

    public void LaunchDisc(Vector3 pDirection, float pForce = 1f)
    {
        currentDisc.LaunchDisc(pDirection, pForce);
        gameState = GameState.DiscIsMoving;
    }

    public Vector3 GetDiscPosition()
    {
        return currentDisc.gameObject.transform.position;
    }

    public void ChangeDisc(GameObject pNewDiscPrefab)
    {
        Destroy(currentDisc.gameObject);
        GameObject go = Instantiate(pNewDiscPrefab);
        currentDisc = go.GetComponentInChildren<SC_DiscMaster>();
        go.transform.position = defaultPos;
        print(currentDisc.name);
    }
}
