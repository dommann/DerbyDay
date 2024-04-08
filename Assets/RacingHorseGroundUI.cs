using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RacingHorseGroundUI : MonoBehaviour
{
    public Image GroundImage;
    public TextMeshProUGUI _horseName;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        SetVisibility(false);
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            _canvas.enabled = true;
        }
        else
        {
            _canvas.enabled = false;
        }
        
    }
}
