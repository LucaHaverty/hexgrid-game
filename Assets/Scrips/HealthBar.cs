using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health healthScript;
    public Image fillImage;
    public Gradient colorGradient;

    private Vector3 scale;
    void Start()
    {
        scale = transform.localScale;
        healthScript.OnHealthUpdate.AddListener(UpdateHealthHar);
        UpdateHealthHar(1);
    }

    private void UpdateHealthHar(float percent)
    {
        if (percent == 1) transform.localScale = Vector3.zero;
        else transform.localScale = scale;
        fillImage.fillAmount = percent;
        fillImage.color = colorGradient.Evaluate(percent);
    }
}
