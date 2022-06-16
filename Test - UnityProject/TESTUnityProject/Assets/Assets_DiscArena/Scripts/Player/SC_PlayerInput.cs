using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerInput : MonoBehaviour
{
    [SerializeField] private SC_GameManager gameManager = null;
    Vector3 inputStartPos = Vector3.zero; // Pos of finger or mouse when player press

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
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Vector3 _Direction = inputStartPos;
                    _Direction.x = Input.GetTouch(0).position.x - inputStartPos.x;
                    _Direction.y = 0f;
                    _Direction.z = Input.GetTouch(0).position.y - inputStartPos.y;
                    _Direction = _Direction.normalized;
                    gameManager.LaunchDisc(_Direction, 4);
                }
            }
#endif

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                inputStartPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 _Direction = (Input.mousePosition - inputStartPos).normalized;
                _Direction.z = _Direction.y;
                _Direction.y = 0f;
                gameManager.LaunchDisc(_Direction, 4);
            }
#endif
        }
    }
}