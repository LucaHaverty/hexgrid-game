using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    void Start()
    {
        text.text = $"Verson {Application.version}";
    }
}
