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

    // Variable set when press start
    // if press is over UI element => canUpdate = false else canUpdate = true
    private bool canUpdate = false;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == SC_GameManager.GameState.WaitToLaunchDisc)
        {
#if (UNITY_ANDROID || UNITY_IOS) //&& !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if(gameManager.IsPointerOverUIElement() == false)
                    {
                        canUpdate = true;
                        inputStartPos = Input.GetTouch(0).position;
                        timerBeforeUpdatePrediction = 0f;
                    }
                    else
                    {
                        canUpdate = false;
                    }                    
                }
                // Update prediction
                else if (canUpdate && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    direction = (Input.mousePosition - inputStartPos);
                    direction.z = direction.y;
                    direction.y = 0f;

                    // Can update prediction
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

                // Launch Disc
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (canUpdate)
                    {
                        if (direction.magnitude > 1f)
                        {
                            movementPrediction.HidePrediction();
                            gameManager.LaunchDisc(direction, gameManager.CurrentDisc.MoveSpeed);
                        }
                    }
                }
            }
#endif

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (gameManager.IsPointerOverUIElement() == false)
                {
                    inputStartPos = Input.mousePosition;
                    timerBeforeUpdatePrediction = 0f;
                    canUpdate = true;
                }
                else
                {
                    canUpdate = false;
                }
            }

            // Launch Disc
            else if (Input.GetMouseButtonUp(0))
            {
                if (canUpdate)
                {
                    if ((Input.mousePosition - inputStartPos).magnitude >= 70)
                    {
                        movementPrediction.HidePrediction();
                        gameManager.LaunchDisc(direction, gameManager.CurrentDisc.MoveSpeed);
                    }
                }
            }

            // Update Prediction
            if (canUpdate && Input.GetMouseButton(0))
            {
                direction = (Input.mousePosition - inputStartPos);
                direction.z = direction.y;
                direction.y = 0f;

                // Can update prediction
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