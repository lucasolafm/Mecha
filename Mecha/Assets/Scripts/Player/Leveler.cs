using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Leveler : MonoBehaviour
{
    public static UnityEvent LeveledUp = new UnityEvent();

    public TextMeshPro levelUpText;
    public TextMeshProUGUI currentLevelText;
    public float showLevelUpTime;
    
    private int _currentLevel;
    private Coroutine _hideLevelUpRoutine;

    void Awake()
    {
        Experience.ReachedMaxXP.AddListener(OnReachedMaxXP);
    }

    void Start()
    {
        _currentLevel = 1;
        currentLevelText.text = "Lvl " + _currentLevel;
    }

    private void OnReachedMaxXP()
    {
        LevelUp();
        LeveledUp.Invoke();
    }

    private void LevelUp()
    {
        _currentLevel++;
        levelUpText.text = "Level up!";
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