using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerInput : MonoBehaviour
{
    [SerializeField] private SC_GameManager gameManager = null;
    [SerializeField] private SC_MovementPrediction movementPrediction = null;
    [SerializeField] private float delayBeforeUpdatePrediction = 0.01f;
    Vector3 inputStartPos = Vector3.zero; // Pos of finger or mouse when player press
    Vector3 direction = Vector3.zero;

    float timerBeforeUpdatePrediction = 0f;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == SC_GameManager.GameState.WaitToLaunchDisc)
        {
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    inputStartPos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    direction = (Input.mousePosition - inputStartPos);
                    direction.z = direction.y;
                    direction.y = 0f;
                    if (timerBeforeUpdatePrediction <= 0f)
                    {
                        movementPrediction.CalculateTrajectory(gameManager.GetDiscPosition(), direction, 0f, 0);
                        timerBeforeUpdatePrediction = delayBeforeUpdatePrediction;
                    }
                    else
                    {
                        timerBeforeUpdatePrediction -= Time.deltaTime;
                    }
                    direction = direction.normalized;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {

                    gameManager.LaunchDisc(direction, gameManager.CurrentDisc.MoveSpeed);
                }
            }
#endif

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                inputStartPos = Input.mousePosition;
                direction = (Input.mousePosition - gameManager.GetDiscPosition()).normalized;
                direction.z = direction.y;
                direction.y = 0f;
                timerBeforeUpdatePrediction = 0f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                gameManager.LaunchDisc(direction, gameManager.CurrentDisc.MoveSpeed);
            }

            if (Input.GetMouseButton(0))
            {
                direction = (Input.mousePosition - inputStartPos);
                direction.z = direction.y;
                direction.y = 0f;
                if (timerBeforeUpdatePrediction <= 0f)
                {
                    movementPrediction.CalculateTrajectory(gameManager.GetDiscPosition(), direction, 0f, 0);
                    timerBeforeUpdatePrediction = delayBeforeUpdatePrediction;
                }
                else
                {
                    timerBeforeUpdatePrediction -= Time.deltaTime;
                }
                direction = direction.normalized;
            }
#endif
        }
    }
}