using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MoveObject : MonoBehaviour
{
    [System.Serializable]
    public class SaveMoveObject
    {
        public Vector3[] checkpoint;
        public float speed = 0f;
        public bool pingPong = false;
    }

    [SerializeField] private Vector3[] checkpoint;
    [SerializeField] private float speed = 0f;

    [SerializeField] private bool pingPong = true;

    private Vector3 direction = Vector3.zero;
    private int posIndex = 1;
    private bool update = true;
    private bool playReverse = false;

    private float distanceFromArrivePoint = 0f;

    private void Update()
    {
        if (update)
        {
            gameObject.transform.position += direction * speed * Time.deltaTime;
            float newDistance = Vector3.Distance(gameObject.transform.position, checkpoint[posIndex]);

            if (distanceFromArrivePoint <= newDistance)
            {
                // new distance exceed last distance
                IsArriveAtDestination();
            }
            else
            {
                distanceFromArrivePoint = newDistance;
                if (newDistance <= 0.2f)
                {
                    IsArriveAtDestination();
                }
            }

        }
    }

    private void IsArriveAtDestination()
    {
        int startIndex = posIndex;
        posIndex += playReverse ? -1 : 1;

        if ((posIndex >= checkpoint.Length && playReverse == false) ||
            (posIndex < 0 && playReverse == true))
        {
            if (pingPong)
            {
                playReverse = !playReverse;
                posIndex = playReverse ? checkpoint.Length - 1 : 0;
            }
            else
            {
                update = false;
            }
        }

        if (update)
        {
            direction = checkpoint[posIndex] - checkpoint[startIndex];
            direction = direction.normalized;
        }

        distanceFromArrivePoint = Vector3.Distance(gameObject.transform.position, checkpoint[posIndex]);
    }

    public void LoadSave(SaveMoveObject save)
    {
        speed = save.speed;
        checkpoint = save.checkpoint;
        pingPong = save.pingPong;

        direction = checkpoint[1] - checkpoint[0];
        direction = direction.normalized;
        gameObject.transform.position = checkpoint[0];
        gameObject.transform.GetChild(0).transform.rotation = Quaternion.LookRotation(direction);

        distanceFromArrivePoint = Vector3.Distance(gameObject.transform.position, checkpoint[posIndex]);
    }

    public SaveMoveObject Save()
    {
        SaveMoveObject save = new SaveMoveObject();

        save.speed = speed;
        save.checkpoint = checkpoint;
        save.pingPong = pingPong;

        return save;
    }
}
