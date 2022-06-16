using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    [SerializeField] private SC_DiscMaster currentDisc = null;

    public enum GameState
    {
        WaitToLaunchDisc,
        DiscIsMoving
    }

    public GameState gameState { get; private set; } = GameState.WaitToLaunchDisc;

    public void LaunchDisc(Vector3 pDirection, float pForce = 1f)
    {
        currentDisc.LaunchDisc(pDirection, pForce);
        gameState = GameState.DiscIsMoving;
    }

    public Vector3 GetDiscPosition()
    {
        return currentDisc.gameObject.transform.position;
    }
}
