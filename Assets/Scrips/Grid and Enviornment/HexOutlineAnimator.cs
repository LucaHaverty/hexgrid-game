using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexOutlineAnimator : MonoBehaviour
{
    public float animationCooldown;
    public float finalOpacity;
    public float moveSpeed;
    public float waveSize;
    public float startPosition;
    
    private GridManager hexGrid;
    private bool animPlaying;

    void Start()
    {
        hexGrid = GridManager.instance;
        StartCoroutine(PlayAnimation());
    }

    /** Sine wave animation to show hex tile outlines */
    IEnumerator PlayAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(animationCooldown);

            float startTime = Time.time;

            while (Time.time - startTime < 7.5f)
            {
                float timeValue = (Time.time - startTime) * moveSpeed * Mathf.PI;
                foreach (HexTile tile in hexGrid.tiles)
                {
                    float positionValue = timeValue - tile.worldPos.x * waveSize + startPosition;
                    positionValue = Mathf.Clamp(positionValue, 0, Mathf.PI);
                    float colorValue = Mathf.Sin(positionValue) * finalOpacity;
                    tile.SetOutlineAlpha(colorValue);
                }
                yield return new WaitForEndOfFrame();
            }

            foreach (HexTile tile in hexGrid.tiles)
            {
                tile.SetOutlineAlpha(0);
            }
        }
    }
}
