﻿using System.Collections;
using UnityEngine;
using TMPro;

public class Leveler : MonoBehaviour
{
    public TextMeshPro levelUpText;
    public TextMeshProUGUI currentLevelText;
    public float showLevelUpTime;

    private Player _player;
    private int _currentLevel;
    private Coroutine _hideLevelUpRoutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        _currentLevel = 1;
        currentLevelText.text = "Lvl " + _currentLevel;
    }
    
    public void LevelUp()
    {
        _currentLevel++;
        _player.ResetXP();
        levelUpText.text = "Level up! (" + _currentLevel + ")";
        currentLevelText.text = "Lvl " + _currentLevel;
        
        if (_hideLevelUpRoutine != null) StopCoroutine(_hideLevelUpRoutine);
        _hideLevelUpRoutine = StartCoroutine(HideLevelUpTextRoutine());
    }
    
    private IEnumerator HideLevelUpTextRoutine()
    {
        yield return new WaitForSeconds(showLevelUpTime);
        
        levelUpText.text = "";
    }
}