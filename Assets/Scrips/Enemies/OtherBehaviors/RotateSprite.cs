using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public Transform spritesParent;
    public float rotateSpeed;
    public float alphaFalloff;
    public bool baseAlphaOffMainSprite;

    private Transform[] children;
    
    void Start()
    {
        children = new Transform[spritesParent.childCount];
        children[0] = spritesParent.GetChild(0);
        float baseAlpha = children[0].GetComponent<SpriteRenderer>().color.a;

        for (int i = 1; i < spritesParent.childCount; i++)
        {
            children[i] = spritesParent.GetChild(i);
            var rend = children[i].GetComponent<SpriteRenderer>();
            Color color = new Color(rend.color.r, rend.color.g, rend.color.b, baseAlpha - i * alphaFalloff);
            rend.color = color;
            rend.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }

    void FixedUpdate() {
        for (int i = 0; i < spritesParent.childCount; i++)
        {
            children[i].Rotate(new Vector3(0, 0, (rotateSpeed + i*25) * Time.fixedDeltaTime));
        }
    }

    public void RecalculateAlphas()
    {
        Color baseColor = children[0].GetComponent<SpriteRenderer>().color;
        for (int i = 1; i < spritesParent.childCount; i++)
        {
            
            var rend = children[i].GetComponent<SpriteRenderer>();
            Color color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a - i * alphaFalloff);
            rend.color = color;
        }
    }
}
