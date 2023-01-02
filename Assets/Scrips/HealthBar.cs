using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health healthScript;
    public Image fillImage;
    public Gradient colorGradient;
    
    void Start()
    {
        healthScript.OnHealthUpdate.AddListener(UpdateHealthHar);
        UpdateHealthHar(1);
    }

    private void UpdateHealthHar(float percent)
    {
        fillImage.fillAmount = percent;
        fillImage.color = colorGradient.Evaluate(percent);
    }
}
