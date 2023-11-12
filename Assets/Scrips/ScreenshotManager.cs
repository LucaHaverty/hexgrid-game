using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private Camera screenshotCam;
    private void Start()
    {
        canvas.SetActive(false);
        screenshotCam = Instantiate(Camera.main.gameObject).GetComponent<Camera>();
        canvas.SetActive(true);
    }

    private void Update() { if (Input.GetKeyDown(KeyCode.F4)) SaveCameraView(screenshotCam); }
    
    public void SaveCameraView(Camera cam)
    {
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        byte[] byteArray = renderedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("C:/Users/lucah/Desktop/GameScreenshots/"+(Random.Range(0, 1000000).ToString())+".png", byteArray);
    }
}
