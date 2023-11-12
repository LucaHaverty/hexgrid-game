using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public static float difficulty = 0.5f;
    
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] diffNames;
    [SerializeField] private Gradient textColor;
    [SerializeField] private Image knobFill;

    void Update()
    {
        int diff = Mathf.FloorToInt(slider.value * 5f) + 1;
        if (slider.value == 0) diff = 0;
        
        text.text = diffNames[diff];

        difficulty = diff / 6f;

        text.color = textColor.Evaluate(difficulty);
        knobFill.color = textColor.Evaluate((slider.value * 5 + 1) / 6);
    }
}
