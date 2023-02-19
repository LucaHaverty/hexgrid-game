using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    
    private bool mouseDown;
    private Vector2 camPosWhenDown;
    private Vector2 mousePosWhenDownScreen;
    private Camera cam;

    private Vector2 clampPos;

    private void Start()
    {
        cam = Camera.main;

        StartCoroutine(lateStart());
    }

    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();
        clampPos = new Vector2(Settings.instance.width * Settings.instance.tileScale / 2f,
            Settings.instance.height * Settings.instance.tileScale / 2f);
        Debug.Log(clampPos);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            mouseDown = true;
            camPosWhenDown = cam.transform.position;
            mousePosWhenDownScreen = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
            mouseDown = false;

        if (mouseDown)
        {
            setCamPos(camPosWhenDown - (((Vector2)Input.mousePosition - mousePosWhenDownScreen)*moveSpeed));
        }
    }

    private void setCamPos(Vector2 pos)
    {
        pos = new Vector2(Mathf.Clamp(pos.x, -clampPos.x, clampPos.x), Mathf.Clamp(pos.y, -clampPos.y, clampPos.y));
        var camTransform = cam.transform;
        camTransform.position = new Vector3(pos.x, pos.y, camTransform.position.z);
    }
}
