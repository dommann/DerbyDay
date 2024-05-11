using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private Vector3 originalPos;
    private float _inputsOriginalPosY; //+225 to open
    private float _progressionOriginalPosX; 
    private float _progressionMaxX = 624;

    [SerializeField] private List<RectTransform> _playerInputsUI;
    [SerializeField] private List<RectTransform> _horseProgressionsUI;
    // Start is called before the first frame update
    private void Awake()
    {
        _inputsOriginalPosY = _playerInputsUI[0].anchoredPosition.y;
        _progressionOriginalPosX = _horseProgressionsUI[0].anchoredPosition.x;
    }

    void Start()
    {
        DisableInputUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateInputUI(int playerIndex)
    {
        for (var index = 0; index < _playerInputsUI.Count; index++)
        {
            if (index != playerIndex)
            {
                continue;
            }
            
            var playerInputUI = _playerInputsUI[index];
            var position = playerInputUI.anchoredPosition;
            position = new Vector2(position.x, _inputsOriginalPosY + 225);
            
            // Use DOTween to animate the anchored position
            playerInputUI.DOAnchorPos(position, 0.5f); // Specify the duration here
        }
    }
    public void DisableInputUI()
    {
        foreach (var playerInputUI in _playerInputsUI)
        {
            var position = playerInputUI.anchoredPosition;
            position = new Vector2(position.x, _inputsOriginalPosY);
            playerInputUI.anchoredPosition = position;
        }
    }

    public void SetHorseProgression(RacingHorse horse, float currentPercentage)
    {
        float targetPosX = Mathf.Lerp(_progressionOriginalPosX, _progressionMaxX, currentPercentage);

        // Use DOTween to animate the anchored position X
        _horseProgressionsUI[horse.HorseIndex].DOAnchorPosX(targetPosX,0); // Specify the duration here
    }
}
