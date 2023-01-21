using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public Transform spritesParent;
    public float rotateSpeed;
    public float alphaFalloff;
    
    void Start()
    {
        for (int i = 1; i < spritesParent.childCount; i++)
        {
            SpriteRenderer rend = spritesParent.GetChild(i).GetComponent<SpriteRenderer>();
            Color color = new Color(rend.color.r, rend.color.g, rend.color.b, 1 - (i * alphaFalloff));
            rend.color = color;
            rend.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }

    void Update()
    {
        
    }
    
    void FixedUpdate() {
        for (int i = 0; i < spritesParent.childCount; i++)
        {
            spritesParent.GetChild(i).Rotate(new Vector3(0, 0, (rotateSpeed + i*25) * Time.fixedDeltaTime));
        }
    }
}
