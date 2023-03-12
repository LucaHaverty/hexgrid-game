using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    To use with ScreenEffectsManager.cs
 */

[System.Serializable]
public class Effect
{
    public Animator animator;

    public string name;
    public float duration;
    public bool startOut;
}
