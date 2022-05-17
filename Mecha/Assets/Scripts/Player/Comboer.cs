using System.Collections;
using TMPro;
using UnityEngine;

public class Comboer : MonoBehaviour
 {
     public TextMeshPro comboText;
     public float showComboTime;
     
     private int _currentCombo;
     private Coroutine _hideComboRoutine;

     void Awake()
     {
         Player.HitFloor.AddListener(OnHitFloor);
     }
     
     public int GetCurrentCombo()
     {
         return _currentCombo;
     }
     
     private void OnHitFloor()
     {
         _currentCombo = 0;
     }
     
     public void IncreaseCombo()
     {
         _currentCombo++;
         comboText.text = _currentCombo + "x";
        
         if (_hideComboRoutine != null) StopCoroutine(_hideComboRoutine);
         _hideComboRoutine = StartCoroutine(HideComboTextRoutine());
     }

     private IEnumerator HideComboTextRoutine()
     {
         yield return new WaitForSeconds(showComboTime);
        
         comboText.text = "";
     }
 }