using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spritesToFlash;
    private float flashSpeed;
    private Color flashColor;
    private Color[] startColors;

    void Start()
    {
        flashSpeed = Settings.instance.damageFlashTime;
        flashColor = Settings.instance.enemyDamageColor;
        
        GetComponent<Health>().OnHealthUpdate.AddListener(OnDamage);

        if (TryGetComponent<InvisEnemy>(out InvisEnemy invisEnemy))
        {
            startColors = new Color[spritesToFlash.Length];
            for (int i = 0; i < spritesToFlash.Length; i++)
            {
                startColors[i] = invisEnemy.revealedColor;
            }
        }
        else
        {
            startColors = new Color[spritesToFlash.Length];
            for (int i = 0; i < spritesToFlash.Length; i++)
            {
                startColors[i] = spritesToFlash[i].color;
            }
        }
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

    private void UpdateColors(float lerpPercent)
    {
        if (TryGetComponent<InvisEnemy>(out InvisEnemy invisEnemy))
            if (!invisEnemy.visible)
                return;
        
        lerpPercent *= 0.8f;
        for (int i = 0; i < spritesToFlash.Length; i++)
        {
            float alpha = spritesToFlash[i].color.a;
            Color newColor = Color.Lerp(startColors[i], flashColor, lerpPercent);
            spritesToFlash[i].color = Utils.ColorWithAlpha(newColor, alpha);
        }
    }
}
