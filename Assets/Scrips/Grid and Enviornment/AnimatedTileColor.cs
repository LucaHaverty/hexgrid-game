using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(HexTile))]
public class AnimatedTileColor : MonoBehaviour
{
    public HexTile hexTile;
    float colorOffset;
    bool paused;

    private void Start()
    {
        colorOffset = Random.Range(0f, Mathf.PI*2);
    }

    void Update()
    {
        if (hexTile.type == null || paused)
            return;

        hexTile.SetColor(hexTile.type.colorRange.Evaluate(GetGradientPosition()));
    }

    public async void SwitchTileColor(Gradient oldColor, Gradient newColor)
    {
        paused = true;
        float blend = 0;
        while (blend < 1)
        {
            blend += Time.deltaTime * Settings.instance.colorSwitchAnimSpeed;
            hexTile.SetColor(Color.Lerp(oldColor.Evaluate(GetGradientPosition()), newColor.Evaluate(GetGradientPosition()), blend));
            await Task.Yield();
        }
        paused = false;
    }

    float GetGradientPosition() { return (Mathf.Sin(Time.time * Settings.instance.animateColorSpeed + colorOffset) + 1) / 2; }
}