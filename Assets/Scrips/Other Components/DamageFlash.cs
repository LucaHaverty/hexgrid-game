using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spritesToFlash;
    private float flashSpeed;
    private Gradient flashColor;

    void Start()
    {
        flashSpeed = Settings.instance.damageFlashTime;
        flashColor = Settings.instance.damageFlashColor;
        
        GetComponent<Health>().OnHealthUpdate.AddListener(OnDamage);
    }

    private bool animPlaying = false;
    private void OnDamage(float currentHealth)
    {
        if (animPlaying)
            return;

        animPlaying = true;
        StartCoroutine(FlashAnimation());
    }

    private IEnumerator FlashAnimation()
    {
        float startTime = Time.time;
        while (Time.time - startTime < flashSpeed)
        {
            float percent = (Time.time - startTime) / flashSpeed;
            UpdateColors(Mathf.Sin(percent*Mathf.PI));
            
            yield return null;
        }

        animPlaying = false;
        UpdateColors(0);
    }

    private void UpdateColors(float gradientPercent)
    {
        foreach (var rend in spritesToFlash)
        {
            rend.color = flashColor.Evaluate(gradientPercent);
        }
    }
}
