using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Transform[] images;
    [SerializeField] private float speed;

    private Vector3 leftPos;
    private float maxX;
    void Start()
    {
        transform.localScale = Vector2.one * (GameManager.instance.levelData.camSize / 5) * transform.localScale.x;
        transform.position = GameManager.instance.levelData.camOffset;

        maxX = -images[0].position.x;
        leftPos = images[0].position;
    }

    private void FixedUpdate()
    {
        foreach (var image in images)
        {
            image.Translate(new Vector2(speed*Time.fixedDeltaTime, 0));
            if (image.transform.position.x > maxX) image.transform.position = leftPos;
        }
    }
}
