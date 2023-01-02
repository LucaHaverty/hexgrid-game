using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRadiusVisualization : MonoBehaviour
{
    public LineRenderer lineRend;
    public SpriteRenderer circleRend;

    public float radius;
    public float fadeTime;
    
    private float circleFinalA;
    private float borderFinalA;

    private int numPoints;

    void Start()
    {
        numPoints = Settings.instance.lineRendCircleNumPoints;
        
        circleFinalA = circleRend.color.a;
        borderFinalA = lineRend.startColor.a;
        
        Utils.GenerateLineRendCircle(lineRend, radius, numPoints);
        StartCoroutine(FadeIn());
    }

    void SetAlphaByPercent(float percent)
    {
        circleRend.color = new Color(circleRend.color.r, circleRend.color.g, circleRend.color.b, circleFinalA*percent);
        Color rendColor = new Color(lineRend.startColor.r, lineRend.startColor.g, lineRend.startColor.b, borderFinalA*percent);
        lineRend.startColor = rendColor;
        lineRend.endColor = rendColor;
    }
    
    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeTime)
        {
            SetAlphaByPercent(t / fadeTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator Fadeout()
    {
        float t = fadeTime;
        while (t > 0)
        {
            SetAlphaByPercent(t / fadeTime);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public void Destroy()
    {
        StartCoroutine(Fadeout());
    }
}
