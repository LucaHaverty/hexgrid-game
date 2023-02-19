using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParalax : MonoBehaviour
{
    [SerializeField] private Transform[] layers;
    [SerializeField] private float[] layerSpeeds;
    private Vector2[] startPos;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        startPos = new Vector2[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            startPos[i] = layers[i].position;
        }
    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //8.8 5
        mousePos = new Vector2(Mathf.Clamp(mousePos.x, -8.8f, 8.8f), Mathf.Clamp(mousePos.y, -5f, 5f));
        
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].position = startPos[i] + (mousePos * -layerSpeeds[i]);
        }
    }
}
