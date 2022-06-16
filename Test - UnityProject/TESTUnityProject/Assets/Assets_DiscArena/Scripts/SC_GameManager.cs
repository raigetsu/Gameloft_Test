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

    public void LaunchDisc(Vector3 Direction, float Force = 1f)
    {
        currentDisc.LaunchDisc(Direction, Force);
        gameState = GameState.DiscIsMoving;
    }
}
