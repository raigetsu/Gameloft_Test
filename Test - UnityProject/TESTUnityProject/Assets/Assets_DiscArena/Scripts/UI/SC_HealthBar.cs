using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage = null;

    public void UpdateHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
        else
            Debug.LogWarning("fillImage is invalid");
    }
}
