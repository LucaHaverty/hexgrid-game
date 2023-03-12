using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public static float difficulty = 1;
    
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] diffNames;
    [SerializeField] private Gradient textColor;

    void Update()
    {
        int diff = Mathf.FloorToInt(slider.value * 5f) + 1;
        if (slider.value == 0) diff = 0;
        
        text.text = diffNames[diff];

        difficulty = diff / 6f;
        text.color = textColor.Evaluate(diff / 6f);
    }
}
