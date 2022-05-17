using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public Image xpBar;
    [SerializeField] private TextMeshProUGUI currentXPText;
    public float maxXP;
    
    private float _currentXP;

    void Start()
    {
        xpBar.fillAmount = 0;
        currentXPText.text = 0 + "%";
    }
    
    public void Shift(float value)
    {
        _currentXP += value;
        xpBar.fillAmount = _currentXP / maxXP;
        currentXPText.text = Mathf.Round(_currentXP) + "%";
    }

    public void Reset()
    {
        Shift(-maxXP);
    }
    
    public bool HasReachedMax()
    {
        return _currentXP >= maxXP;
    }
}