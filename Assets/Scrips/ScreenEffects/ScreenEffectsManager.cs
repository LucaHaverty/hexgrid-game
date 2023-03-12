using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/*
    Requires:
        Effect.cs
 */

public class ScreenEffectsManager : MonoBehaviour
{
    public static ScreenEffectsManager instance;

    public Effect effect;

    void Awake()
    {
        instance = this;
    }

    public void AnimateOut()
    {
        if (effect.animator == null)
            return;

        effect.animator.SetTrigger("Trigger");
    }


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneDelay(sceneIndex));
    }

    IEnumerator LoadSceneDelay(int sceneIndex)
    {
        AnimateOut();
        yield return new WaitForSeconds(effect.duration*3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
