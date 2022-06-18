using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RotateObject : MonoBehaviour
{
    [System.Serializable]
    public class SaveRotateObject
    {
        public Vector3 startRotation;
        public Vector3 endRotation;
        public float speed = 0f;
        public float delayBetweenEachRotation = 0.5f;
        public bool useRotateObject = false;
    }

    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float delayBetweenEachRotation = 0.5f;

    private float lerpTimer = 0f;
    private float currentDelay = 0f;
    private void Update()
    {
        if (currentDelay <= 0f)
        {
            lerpTimer += Time.deltaTime * speed;
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, lerpTimer));

            if (lerpTimer >= 1f)
            {
                RotationOver();
            }
        }
        else
        {
            currentDelay -= Time.deltaTime;
        }
    }

    private void RotationOver()
    {
        Vector3 _StartRotation = startRotation;
        startRotation = endRotation;
        endRotation = _StartRotation;
        lerpTimer = 0f;
        currentDelay = delayBetweenEachRotation;
    }

    public void LoadSave(SaveRotateObject save)
    {
        speed = save.speed;
        startRotation = save.startRotation;
        endRotation = save.endRotation;
    }

    public SaveRotateObject Save()
    {
        SaveRotateObject save = new SaveRotateObject();

        save.speed = speed;
        save.startRotation = startRotation;
        save.endRotation = endRotation;
        save.delayBetweenEachRotation = delayBetweenEachRotation;
        save.useRotateObject = true;

        return save;
    }
}
