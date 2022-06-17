using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text = null;
    [SerializeField] private float delayBeforeDestruction = 1f;
    [SerializeField] private float moveSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delayBeforeDestruction);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp(text.color.a - Time.deltaTime * (1 / delayBeforeDestruction), 0f, 250f));
    }
}
