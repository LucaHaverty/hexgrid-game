using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuParalax : MonoBehaviour
{
    [SerializeField] private Transform background;
    [SerializeField] private float layerSpeed; //3
    [SerializeField] private ScreenEffectsManager sem;
    private Vector2 startPos;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        startPos = background.position;
    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //8.8 5
        mousePos = new Vector2(Mathf.Clamp(mousePos.x, -8.8f, 6f), Mathf.Clamp(mousePos.y, -5f, 5f));
        
        background.localPosition = (mousePos * -layerSpeed);
    }

    public void StartLevel(LevelData levelData)
    {
        LevelDataHolder.data = levelData;
        sem.LoadScene(1);
    }
}
